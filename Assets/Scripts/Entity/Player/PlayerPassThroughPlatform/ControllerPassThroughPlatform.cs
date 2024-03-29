using System.Collections.Generic;
using System.Linq;
using TarodevController;
using UnityEngine;
using UnityEngine.InputSystem;
using UI.Menu.Play.Pause;

namespace Player
{
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// Right now it only contains movement and jumping, but it should be pretty easy to expand... I may even do it myself
    /// if there's enough interest. You can play and compete for best times here: https://tarodev.itch.io/
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/GqeHHnhHpz
    /// </summary>
    public class ControllerPassThroughPlatform : MonoBehaviour, IPlayerController
    {
        // Public for external hooks
        public Vector3 Velocity { get; private set; }

        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool Grounded => _colDown;

        private bool _isDead = false;

        private Vector3 _lastPosition;
        private float _currentHorizontalSpeed, _currentVerticalSpeed;

        public static bool isLeftDirection;

        // This is horrible, but for some reason colliders are not fully established when update starts...
        private bool _active;

        private void Awake()
        {
            Invoke(nameof(Activate), 0.5f);
        }

        private void OnEnable()
        {
            isLeftDirection = true;
            global::Player.Health.OnPlayerDied += StopAllPlayerMovements;
        }

        private void OnDisable()
        {
            global::Player.Health.OnPlayerDied -= StopAllPlayerMovements;
        }

        private void Update()
        {
            if (!_active) return;

            if (_isDead) return;

            if (PauseMenu.Instance.IsPaused) return;
            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            GatherInput();
            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
        }

        private void Activate()
        {
            _active = true;
        }

        private void StopAllPlayerMovements()
        {
            _isDead = true;
        }

        #region Gather Input

        private bool _pressedJump;
        private bool _releasedJump;
        private float _playerMoveDirection;

        public void PressJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _pressedJump = true;
            }
            else if (context.canceled)
            {
                _pressedJump = false;
            }
        }

        public void ReleaseJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _releasedJump = true;
            }
            else if (context.canceled)
            {
                _releasedJump = false;
            }
        }

        public void PlayerMovement(InputAction.CallbackContext context)
        {
            _playerMoveDirection = context.ReadValue<float>();
            if (_playerMoveDirection > 0 && !isLeftDirection)
                isLeftDirection = true;
            else if (_playerMoveDirection < 0 && isLeftDirection)
                isLeftDirection = false;
        }

        private void GatherInput()
        {
            Input = new FrameInput
            {
                JumpDown = _pressedJump,
                JumpUp = _releasedJump,
                X = _playerMoveDirection
            };
            if (Input.JumpDown)
            {
                _lastJumpPressed = Time.time;
            }
        }

        #endregion Gather Input

        #region Collisions

        [Header("COLLISION")][SerializeField] private Bounds _characterBounds;
        [SerializeField] private LayerMask _allGroundsLayers;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _platformLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField][Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground
        private bool _isPlatformEffected = true;

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        private bool _colUp, _colRight, _colDown, _colLeft;

        private float _timeLeftGrounded;

        // Tests if PlatformEffector2D is disabling collision between the player and the platform
        private void TestPlatformEffector2D()
        {
            if (Velocity.y > 0f)
            {
                //Velocity = new Vector3(Velocity.x, 0f , Velocity.z);
                _isPlatformEffected = true;
            }
            else
            {
                _isPlatformEffected = !_testPlatformOverlap;
            }
        }

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks()
        {
            TestPlatformEffector2D();
            // Generate ray ranges.
            CalculateRayRanged();

            // Ground
            LandingThisFrame = false;
            var groundedCheck = RunDetection(_raysDown, false);
            if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
            else if (!_colDown && groundedCheck)
            {
                _coyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;
            }

            _colDown = groundedCheck;

            // The rest
            _colUp = RunDetection(_raysUp, false);
            _colLeft = RunDetection(_raysLeft, true);
            _colRight = RunDetection(_raysRight, true);

            bool RunDetection(RayRange range, bool isLeftOrRight)
            {
                bool platformTest = EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _platformLayer));
                bool groundTest = EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));

                if (_isPlatformEffected || isLeftOrRight)
                {
                    platformTest = false;
                }
                //Debug.Log("plat: " + platformTest + " ground: " + groundTest);
                return groundTest || platformTest;
            }
        }

        private void CalculateRayRanged()
        {
            // This is crying out for some kind of refactor.
            var b = new Bounds(transform.position, _characterBounds.size);

            _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
        }

        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        #endregion Collisions

        #region Walk

        [Header("WALKING")][SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;

        private void CalculateWalk()
        {
            if (Input.X != 0)
            {
                // Set horizontal move speed
                _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else
            {
                // No input. Let's slow the character down
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }

            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
            {
                // Don't walk through walls
                _currentHorizontalSpeed = 0;
            }
        }

        #endregion Walk

        #region Gravity

        [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity()
        {
            if (_colDown)
            {
                // Move out of the ground
                if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion Gravity

        #region Jump

        [Header("JUMPING")][SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed;
        private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!_colDown)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
            {
                _apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (Input.JumpDown && CanUseCoyote || HasBufferedJump)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
            }
            else
            {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0)
            {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            if (_colUp)
            {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
        }

        #endregion Jump

        #region Move

        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;
        private bool _testPlatformOverlap = false;
        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            var pos = transform.position;
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            if (Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _platformLayer))
                _testPlatformOverlap = true;
            else
                _testPlatformOverlap = false;

            // check furthest movement. If nothing hit, move and don't do extra checks
            Collider2D hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _allGroundsLayers);
            if (_isPlatformEffected)
                hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);

            if (!hit || Grounded)
            {
                transform.position += move;
                return;
            }
            Debug.Log("Afection: " + _isPlatformEffected + " !_testPlatform: " + !_testPlatformOverlap);
            // otherwise increment away from current pos; see what closest position we can move to
            var positionToMoveTo = transform.position;
            for (int i = 1; i < _freeColliderIterations; i++)
            {
                // increment to check all but furthestPoint - we did that already
                var t = (float)i / _freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                bool overlapTest = Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _allGroundsLayers);
                if (_isPlatformEffected)
                    overlapTest = Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer);
                if (overlapTest)
                {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1)
                    {
                        if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                positionToMoveTo = posToTry;
            }
        }

        #endregion Move
    }
}