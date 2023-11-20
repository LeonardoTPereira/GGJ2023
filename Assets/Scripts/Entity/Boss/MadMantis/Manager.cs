using Assets.Scripts.Effects;
using Spriter2UnityDX;
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

        [SerializeField] private float fadeCooldown = 1f;

        private Animator _animator;
        private bool _isJumping;
        private bool _isDead;
        private Coroutine _attackRoutine;
        private Coroutine _jumpRoutine;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _jumpTarget;

        private const string verticalAttackSFXName = "MantisVerticalAttack";
        private const string horizontalAttackSFXName = "MantisHorizontalAttack";
        private const string damageSFXName = "MantisDamage";
        private const string deathSFXName = "MantisDeath";
        private const string verticalBulletSFXName = "MantisVerticalBullet";
        private const string mantisRageSFXName = "MantisRage";
        private const string MantisOST = "MantisOST";

        [SerializeField] private ParticleSystem deathParticle;
        [SerializeField] private ParticleSystem damageParticle;
        [SerializeField] private EffectsManager effectsManager;

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
            Debug.Log("Start Death");
            StopAttackRoutine();
            _animator.SetTrigger("Death");
            _isDead = true;
            effectsManager.ChangeMantisColorToRed();
            //PlayDeathParticle();
            //effectsManager.BeginDeath();
            AudioManager.Instance.PlaySFX(deathSFXName);
        }

        public void StartFall()
        {
            _rigidbody2D.gravityScale = 9.8f;
        }

        public void ChangeToNextScene()
        {
            UI.LevelChanger.NextSceneSetter.Instance.SetNextScene(fadeCooldown);    // Pass to the next level
        }

        public void StartRage()
        {
            AudioManager.Instance.PlaySFX(mantisRageSFXName);
            IsEnraged = true;
            StopAttackRoutine();
            _animator.SetBool("Enraged", true);
        }

        public void StartFinalStageTransition()
        {
            StopAttackRoutine();
            StopJumpRoutine();
            AudioManager.Instance.PlaySFX(mantisRageSFXName);

            BossFinalStageEffect.Instance.StartFinalBossStageEffect();
            BossFinalStageEffect.Instance.hasFinishedEffects.AddListener(StartFly);
        }

        private void StartFly()
        {
            IsFlying = true;
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
            StopAttackRoutine();
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
            if (_isDead) return;

            if (_isJumping)
            {
                if (IsFlying)
                {
                    if (Mathf.Abs(transform.position.x - RoomCenterX) >= 1) return;
                    _rigidbody2D.gravityScale = 0;
                    _rigidbody2D.velocity = Vector2.zero;
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
            Instantiate(VerticalRightAttackPrefab, UpLeftClawPosition.position, transform.rotation).GetComponent<SpriteRenderer>().flipX = true;
        }

        public void SpawnDoubleVerticalAttackBottom()
        {
            AudioManager.Instance.PlaySFX(verticalAttackSFXName);
            Instantiate(VerticalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
            Instantiate(VerticalRightAttackPrefab, LeftClawPosition.position, transform.rotation).GetComponent<SpriteRenderer>().flipX = true;
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
            Instantiate(HorizontalRightAttackPrefab, LeftClawPosition.position, transform.rotation).GetComponent<SpriteRenderer>().flipX = true;
        }

        public void SpawnDoubleHorizontalAttackTop()
        {
            AudioManager.Instance.PlaySFX(horizontalAttackSFXName);
            Instantiate(HorizontalLeftAttackPrefab, UpRightClawPosition.position, transform.rotation);
            Instantiate(HorizontalRightAttackPrefab, UpLeftClawPosition.position, transform.rotation).GetComponent<SpriteRenderer>().flipX = true;
        }

        internal void PlayDamageSound()
        {
            AudioManager.Instance.PlaySFX(damageSFXName);
        }

        internal void PlayDamageParticle()
        {
            damageParticle.Play();
        }

        public void PlayDamageParticle(Transform transform, Vector3 normal)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, normal.z + 90);
            if (normal.x < 0)
            {
                rotation = Quaternion.Euler(0, 0, normal.z - 90);
            }
            var instanceDamageParticle = Instantiate(damageParticle, transform.position, rotation);
            instanceDamageParticle.Play();
        }

        internal void PlayDeathParticle()
        {
            deathParticle.Play();
        }

        private void StopAttackRoutine()
        {
            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }
        }

        private void StopJumpRoutine()
        {
            if (_jumpRoutine != null)
            {
                StopCoroutine(_jumpRoutine);
                _animator.ResetTrigger("Jump");
                _jumpRoutine = null;
            }
        }
    }
}