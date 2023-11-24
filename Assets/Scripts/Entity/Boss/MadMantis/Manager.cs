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
        private float _verticalProbabilityBoost;
        private float _topProbabilityBoost;

        private const string verticalAttackSFXName = "MantisVerticalAttack";
        private const string horizontalAttackSFXName = "MantisHorizontalAttack";
        private const string damageSFXName = "MantisDamage";
        private const string deathSFXName = "MantisDeath";
        private const string verticalBulletSFXName = "MantisVerticalBullet";
        private const string mantisRageSFXName = "MantisRage";
        private const string MantisOST = "MantisOST";
        private const float probabilityBoost = 0.25f;
        private const float baseProbability = 0.5f;
        private const float flyingMaxCooldownBoost = 0.5f;
        private const float flyingMinCooldownBoost = 0.1f;

        [Header("Visual Effects")]
        [SerializeField] private ParticleSystem deathParticle;

        [SerializeField] private ParticleSystem damageParticle;
        [SerializeField] private ParticleSystem finalDamageParticle;
        [SerializeField] private EffectsManager effectsManager;

        private void Awake()
        {
            IsEnraged = false;
            IsFlying = false;
            _isJumping = false;
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _verticalProbabilityBoost = 0f;
            _topProbabilityBoost = 0f;
        }

        private void Start()
        {
            AudioManager.Instance.PlayMusic(MantisOST);
        }

        public void StartAttack(float minCooldownBoost, float maxCooldownBoost)
        {
            _attackRoutine ??= StartCoroutine(AttackLoop(minCooldownBoost, maxCooldownBoost));
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
            StartAttack(flyingMinCooldownBoost, flyingMaxCooldownBoost);
        }

        public void StartDeath()
        {
            Debug.Log("Start Death");
            StopAttackRoutine();
            _animator.SetTrigger("Death");
            AudioManager.Instance.PlaySFX(deathSFXName);
            _isDead = true;
            effectsManager.ChangeMantisColorToRed();
            //PlayDeathParticle();
            //effectsManager.BeginDeath();
        }

        public void StartFall()
        {
            _rigidbody2D.gravityScale = 9.8f;
            finalDamageParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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

        private IEnumerator AttackLoop(float minCooldownBoost, float maxCooldownBoost)
        {
            while (true)
            {
                var minCooldown = Mathf.Max(MinAttackCooldown - minCooldownBoost, 0.1f);
                var maxCooldown = Mathf.Max(MaxAttackCooldown - maxCooldownBoost, MinAttackCooldown);
                var coolDown = Random.Range(minCooldown, maxCooldown);
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
            if (Random.value < (baseProbability + _verticalProbabilityBoost))
            {
                _verticalProbabilityBoost -= probabilityBoost;
                StartVerticalAttackAnimation();
            }
            else
            {
                _verticalProbabilityBoost += probabilityBoost;
                StartHorizontalAttackAnimation();
            }
        }

        private void StartVerticalAttackAnimation()
        {
            if (Random.value < (baseProbability + _topProbabilityBoost))
            {
                _topProbabilityBoost -= probabilityBoost;
                _animator.SetTrigger("VerticalAttackTop");
            }
            else
            {
                _topProbabilityBoost += probabilityBoost;
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
            if (Random.value < (baseProbability + _topProbabilityBoost))
            {
                _topProbabilityBoost -= probabilityBoost;
                _animator.SetTrigger("HorizontalAttackTop");
            }
            else
            {
                _topProbabilityBoost += probabilityBoost;
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

        internal void PlayFinalDamageParticle()
        {
            finalDamageParticle.Play();
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