using System.Collections;
using UnityEngine;

namespace Boss.MadMantis
{
    public class Manager : MonoBehaviour
    {
        [field: SerializeField] public float MinAttackCooldown { get; private set; }
        [field: SerializeField] public float MaxAttackCooldown { get; private set; }
        [field: SerializeField] public float JumpHeight { get; private set; }
        [field: SerializeField] public float MinFlyHeight { get; private set; }
        [field: SerializeField] public int RoomCenterX { get; private set; }
        [field: SerializeField] public GameObject VerticalLeftAttackPrefab { get; private set; }
        [field: SerializeField] public GameObject VerticalRightAttackPrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalLeftAttackPrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalRightAttackPrefab { get; private set; }
        [field: SerializeField] public Transform LeftClawPosition { get; private set; }
        [field: SerializeField] public Transform RightClawPosition { get; private set; }
        [field: SerializeField] public Transform UpRightClawPosition { get; private set; }
        [field: SerializeField] public Transform UpLeftClawPosition { get; private set; }
        [field: SerializeField] public Transform PlayerTransform { get; private set; }

        [field: SerializeField] public bool IsEnraged { get; private set; }
        [field: SerializeField] public bool IsFlying { get; private set; }
        [SerializeField] private float fadeCooldown = 3f;
        private Animator _animator;
        private bool _isJumping;
        private Coroutine _attackRoutine;
        private Coroutine _jumpRoutine;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _jumpTarget;

        private const string verticalAttackSFXName = "MantisVerticalAttack";
        private const string horizontalAttackSFXName = "MantisHorizontalAttack";
        private const string damageSFXName = "MantisDamage";
        private const string deathSFXName = "MantisDeath";
        private const string verticalBulletSFXName = "MantisVerticalBullet";
        private const string MantisOST = "MantisOST";

        private void Awake()
        {
            IsEnraged = false;
            IsFlying = false;
            _isJumping = false;
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            AudioManager.Instance.PlayMusic(MantisOST);
        }

        public void StartAttack()
        {
            _attackRoutine ??= StartCoroutine(AttackLoop());
        }

        public void StartJump()
        {
            _jumpRoutine ??= StartCoroutine(JumpRoutine());
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void Fly()
        {
            StartAttack();
        }

        public void StartDeath()
        {
            StopCoroutine(_attackRoutine);
            AudioManager.Instance.PlaySFX(deathSFXName);
            _animator.SetTrigger("Death");
        }

        public void ChangeToNextScene()
        {
            UI.LevelChanger.NextSceneSetter.Instance.SetNextScene(fadeCooldown);    // Pass to the next level
        }

        public void StartRage()
        {
            IsEnraged = true;
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
            _animator.SetBool("Enraged", true);
        }

        public void StartFly()
        {
            IsFlying = true;
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
            StopCoroutine(_jumpRoutine);
            _jumpRoutine = null;
            _rigidbody2D.velocity = new Vector2(RoomCenterX - transform.position.x, 0);
            _rigidbody2D.AddForce(new Vector2(0, JumpHeight), ForceMode2D.Impulse);
            _isJumping = true;
            _animator.SetBool("Flying", true);
        }

        private IEnumerator AttackLoop()
        {
            while (true)
            {
                var coolDown = Random.Range(MinAttackCooldown, MaxAttackCooldown);
                yield return new WaitForSeconds(coolDown);
                Attack();
            }
        }

        private IEnumerator JumpRoutine()
        {
            var coolDown = Random.Range(5 * MinAttackCooldown, 5 * MaxAttackCooldown);
            yield return new WaitForSeconds(coolDown);
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
            Jump();
        }

        private void Jump()
        {
            _animator.SetTrigger("Jump");
            _isJumping = true;
            _jumpTarget = PlayerTransform.position;
            _rigidbody2D.velocity = new Vector2(_jumpTarget.x - transform.position.x, 0);
            _rigidbody2D.AddForce(new Vector2(0, JumpHeight), ForceMode2D.Impulse);
            _jumpRoutine = null;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsEnraged) return;
            if (!collision.gameObject.CompareTag("Block")) return;
            _isJumping = false;
        }

        private void FixedUpdate()
        {
            if (_isJumping)
            {
                if (IsFlying)
                {
                    if (Mathf.Abs(transform.position.x - RoomCenterX) >= 1) return;
                    _rigidbody2D.gravityScale = 0;
                    _isJumping = false;
                }
                else
                {
                    if (Mathf.Abs(transform.position.x - _jumpTarget.x) >= 1) return;
                }
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
            else if (IsFlying)
            {
                var sinHeight = Mathf.Sin(Time.time) * 4;
                var newHeight = sinHeight + MinFlyHeight;
                var newPosition = new Vector2(transform.position.x, newHeight);
                transform.position = newPosition;
            }
        }

        private void Attack()
        {
            if (Random.value < 0.5f)
            {
                StartVerticalAttackAnimation();
            }
            else
            {
                StartHorizontalAttackAnimation();
            }
        }

        private void StartVerticalAttackAnimation()
        {
            if (Random.value < 0.5f)
            {
                _animator.SetTrigger("VerticalAttackTop");
            }
            else
            {
                _animator.SetTrigger("VerticalAttackBottom");
            }
        }

        public void SpawnLeftVerticalAttackTop()
        {
            AudioManager.Instance.PlaySFX(verticalAttackSFXName);
            Instantiate(VerticalLeftAttackPrefab, UpRightClawPosition.position, transform.rotation);
        }

        public void SpawnLeftVerticalAttackBottom()
        {
            AudioManager.Instance.PlaySFX(verticalAttackSFXName);
            Instantiate(VerticalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
        }

        public void SpawnDoubleVerticalAttackTop()
        {
            AudioManager.Instance.PlaySFX(verticalAttackSFXName);
            Instantiate(VerticalLeftAttackPrefab, UpRightClawPosition.position, transform.rotation);
            Instantiate(VerticalRightAttackPrefab, UpLeftClawPosition.position, transform.rotation);
        }

        public void SpawnDoubleVerticalAttackBottom()
        {
            AudioManager.Instance.PlaySFX(verticalAttackSFXName);
            Instantiate(VerticalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
            Instantiate(VerticalRightAttackPrefab, LeftClawPosition.position, transform.rotation);
        }

        private void StartHorizontalAttackAnimation()
        {
            if (Random.value < 0.5f)
            {
                _animator.SetTrigger("HorizontalAttackTop");
            }
            else
            {
                _animator.SetTrigger("HorizontalAttackBottom");
            }
        }

        public void SpawnLeftHorizontalAttackTop()
        {
            AudioManager.Instance.PlaySFX(horizontalAttackSFXName);
            Instantiate(HorizontalLeftAttackPrefab, UpRightClawPosition.position, transform.rotation);
        }

        public void SpawnLeftHorizontalAttackBottom()
        {
            AudioManager.Instance.PlaySFX(horizontalAttackSFXName);
            Instantiate(HorizontalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
        }

        public void SpawnDoubleHorizontalAttackBottom()
        {
            Instantiate(HorizontalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
            Instantiate(HorizontalRightAttackPrefab, LeftClawPosition.position, transform.rotation);
        }

        public void SpawnDoubleHorizontalAttackTop()
        {
            AudioManager.Instance.PlaySFX(horizontalAttackSFXName);
            Instantiate(HorizontalLeftAttackPrefab, UpRightClawPosition.position, transform.rotation);
            Instantiate(HorizontalRightAttackPrefab, UpLeftClawPosition.position, transform.rotation);
        }

        internal void PlayDamageSound()
        {
            AudioManager.Instance.PlaySFX(damageSFXName);
        }
    }
}