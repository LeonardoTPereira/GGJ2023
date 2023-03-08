using System.Collections;
using Entity;
using TarodevController;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    /// <summary>
    /// This is a pretty filthy script. I was just arbitrarily adding to it as I went.
    /// You won't find any programming prowess here.
    /// This is a supplementary script to help with effects and animation. Basically a juice factory.
    /// </summary>
    public class Animator : MonoBehaviour {
        [SerializeField] private UnityEngine.Animator _anim;
        [SerializeField] private AudioSource _source;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private ParticleSystem _jumpParticles, _launchParticles;
        [SerializeField] private ParticleSystem _moveParticles, _landParticles;
        [SerializeField] private AudioClip[] _footsteps;
        [SerializeField] private float _maxTilt = .1f;
        [SerializeField] private float _tiltSpeed = 1;
        [SerializeField, Range(1f, 3f)] private float _maxIdleSpeed = 2;
        [SerializeField] private float _maxParticleFallSpeed = -40;

        private IPlayerController _player;
        private TransformController _transformController;
        private bool _playerGrounded;
        private ParticleSystem.MinMaxGradient _currentGradient;
        private Vector2 _movement;
        private bool _isBlinking = false;
        private bool _isRunning = false;
        private bool _isDead = false;
        private float _lastInput;

        private Coroutine blinkCorroutine;
        private Coroutine runCorroutine;

        private void Awake()
        {
            _player = GetComponentInParent<IPlayerController>();
            _transformController = GetComponentInParent<TransformController>();
            _lastInput = 1;
        } 

        private IEnumerator BlinkingLoop()
        {
            _isBlinking = true;
            float randomBlinking = Random.Range(5, 10);
            _anim.SetTrigger("isBlinking");
            yield return new WaitForSeconds(randomBlinking);
            _isBlinking = false;

        }

        private IEnumerator RunningLoop()
        {
            _isRunning = true;
            yield return new WaitForSeconds(6);
            _anim.SetTrigger("Running");
            _isRunning = false;

        }


        void Update() {

            if (_player == null) return;
            if (_isDead) return;
            
            // Flip the object
            if (_player.Input.X != 0)
            {
                if (_player.Input.X * _lastInput < 0)
                {
                    _transformController.Flip();
                    _lastInput = _player.Input.X;
                    Debug.Log("Flip Player");
                }
                    
            }

            // Speed up idle while running
            if (Mathf.Abs(_player.Input.X) > 0.01)
            {
                _isBlinking = false;
                StopCoroutine(blinkCorroutine);
                _anim.SetBool("isWalking", true);

                if (!_isRunning)
                    runCorroutine = StartCoroutine(RunningLoop());
            }
            else
            {
                _anim.SetBool("isWalking", false);
                if (!_isBlinking)
                    blinkCorroutine = StartCoroutine(BlinkingLoop());
            }

            // Splat
            if (_player.LandingThisFrame) {
                _anim.SetTrigger(GroundedKey);
                _source.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
            }

            // Jump effects
            if (_player.JumpingThisFrame) {
                _anim.SetTrigger(JumpKey);
                _anim.ResetTrigger(GroundedKey);

                // Only play particles when grounded (avoid coyote)
                if (_player.Grounded) {
                    SetColor(_jumpParticles);
                    SetColor(_launchParticles);
                    _jumpParticles.Play();
                }
            }

            // Play landing effects and begin ground movement effects
            if (!_playerGrounded && _player.Grounded) {
                _playerGrounded = true;
                _moveParticles.Play();
                _landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, _maxParticleFallSpeed, _movement.y);
                SetColor(_landParticles);
                _landParticles.Play();
            }
            else if (_playerGrounded && !_player.Grounded) {
                _playerGrounded = false;
                _moveParticles.Stop();
            }

            // Detect ground color
            var groundHit = Physics2D.Raycast(transform.position, Vector3.down, 2, _groundMask);
            if (groundHit && groundHit.transform.TryGetComponent(out SpriteRenderer r)) {
                _currentGradient = new ParticleSystem.MinMaxGradient(r.color * 0.9f, r.color * 1.2f);
                SetColor(_moveParticles);
            }

            _movement = _player.RawMovement; // Previous frame movement is more valuable

        }

        private void StopAllPlayerAnimations()
        {
            _isDead = true;
        }

        private void OnDisable() {
            _moveParticles.Stop();
            global::Player.Health.OnPlayerDied -= StopAllPlayerAnimations;
        }

        private void OnEnable() {
            _moveParticles.Play();
            global::Player.Health.OnPlayerDied += StopAllPlayerAnimations;
        }

        void SetColor(ParticleSystem ps) {
            var main = ps.main;
            main.startColor = _currentGradient;
        }

        #region Animation Keys

        private static readonly int GroundedKey = UnityEngine.Animator.StringToHash("Grounded");
        private static readonly int IdleSpeedKey = UnityEngine.Animator.StringToHash("IdleSpeed");
        private static readonly int JumpKey = UnityEngine.Animator.StringToHash("Jump");
        private static readonly int SpeedKey = UnityEngine.Animator.StringToHash("Speed");

        #endregion
    }
}