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
        private Animator _animator;
        private bool _isJumping;
        private Coroutine _attackRoutine;
        private Coroutine _jumpRoutine;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _jumpTarget;

        private void Awake()
        {
            IsEnraged = false;
            IsFlying = false;
            _isJumping = false;
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
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
            _animator.SetTrigger("Death");
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
                HorizontalAttack();
            }
        }

        private void StartVerticalAttackAnimation()
        {
            _animator.SetTrigger("VerticalAttack");
        }

        public void SpawnLeftVerticalAttack()
        {
            Instantiate(VerticalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
        }

        public void SpawnDoubleVerticalAttack()
        {
            Instantiate(VerticalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
            Instantiate(VerticalRightAttackPrefab, LeftClawPosition.position, transform.rotation);
        }

        private void HorizontalAttack()
        {
            _animator.SetTrigger("HorizontalAttack");
        }

        public void SpawnLeftHorizontalAttack()
        {
            Instantiate(HorizontalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
        }

        public void SpawnDoubleHorizontalAttack()
        {
            Instantiate(HorizontalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
            Instantiate(HorizontalRightAttackPrefab, LeftClawPosition.position, transform.rotation);
        }
    }
}