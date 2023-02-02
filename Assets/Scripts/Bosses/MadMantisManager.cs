using System.Collections;
using UnityEngine;
using MyBox;

namespace Assets.Scripts.Bosses
{
    public class MadMantisManager : MonoBehaviour
    {
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public int EnragedHP { get; private set; }
        [field: SerializeField] public int FlyingHP { get; private set; }
        [field: SerializeField] public float InvincibilityTime { get; private set; }
        [field: SerializeField] public float MinAttackCooldown { get; private set; }
        [field: SerializeField] public float MaxAttackCooldown { get; private set; }
        [field: SerializeField] public GameObject VerticalLeftAttackPrefab { get; private set; }
        [field: SerializeField] public GameObject VerticalRightAttackPrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalLeftAttackPrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalRightAttackPrefab { get; private set; }
        [field: SerializeField] public Transform LeftClawPosition { get; private set; }
        [field: SerializeField] public Transform RightClawPosition { get; private set; }
        [field: SerializeField] public Transform UpRightClawPosition { get; private set; }
        [field: SerializeField] public Transform UpLeftClawPosition { get; private set; }
        private int _currentHP;
        private Animator _animator;
        private bool _isInvincible;
        private bool _isEnraged;
        private bool _isFlying;
        private Coroutine _attackRoutine;
        private Coroutine _jumpRoutine;

        private void Awake()
        {
            _currentHP = MaxHP;
            _isInvincible = false;
            _isEnraged = false;
            _isFlying = false;
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(int damage)
        {
            if (_isInvincible) return;
            _currentHP -= damage;
            if (_currentHP < EnragedHP && !_isEnraged)
            {
                StartRage();
            }
            else if (_currentHP < FlyingHP && !_isFlying)
            {
                StartFly();
            }
            else if (_currentHP <= 0)
            {
                StartDeath();
            }
            else
            {
                StartCoroutine(InvincibilityCooldown());
            }
        }

#if UNITY_EDITOR

        [ButtonMethod]
        public void OneDamage()
        {
            TakeDamage(1);
        }

#endif

        public void StartAttack()
        {
            _attackRoutine = StartCoroutine(AttackLoop());
        }

        public void StartJump()
        {
            _jumpRoutine = StartCoroutine(JumpRoutine());
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void Fly()
        {
            Debug.Log("Flying");
            StartAttack();
        }

        private void StartDeath()
        {
            _animator.SetTrigger("Death");
        }

        private IEnumerator InvincibilityCooldown()
        {
            yield return new WaitForSeconds(InvincibilityTime);
            _isInvincible = false;
        }

        private void StartRage()
        {
            _isEnraged = true;
            StopCoroutine(_attackRoutine);
            _animator.SetBool("Enraged", true);
        }

        private void StartFly()
        {
            _isFlying = true;
            StopCoroutine(_attackRoutine);
            StopCoroutine(_jumpRoutine);
            _animator.SetBool("Flying", true);
        }

        private IEnumerator AttackLoop()
        {
            while (true)
            {
                var coolDown = Random.Range(MinAttackCooldown, MaxAttackCooldown);
                yield return new WaitForSeconds(coolDown);
                if (_isEnraged)
                {
                    EnragedAttack();
                }
                else
                {
                    Attack();
                }
            }
        }

        private IEnumerator JumpRoutine()
        {
            var coolDown = Random.Range(5 * MinAttackCooldown, 5 * MaxAttackCooldown);
            yield return new WaitForSeconds(coolDown);
            StopCoroutine(_attackRoutine);
            Jump();
        }

        private void Jump()
        {
            _animator.SetTrigger("Jump");
        }

        private void Attack()
        {
            if (Random.value < 0.5f)
            {
                VerticalAttack();
            }
            else
            {
                HorizontalAttack();
            }
        }

        private void EnragedAttack()
        {
            if (Random.value < 0.5f)
            {
                DoubleVerticalAttack();
            }
            else
            {
                DoubleHorizontalAttack();
            }
        }

        private void VerticalAttack()
        {
            Instantiate(VerticalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
        }

        private void DoubleVerticalAttack()
        {
            Instantiate(VerticalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
            Instantiate(VerticalRightAttackPrefab, LeftClawPosition.position, transform.rotation);
        }

        private void HorizontalAttack()
        {
            Instantiate(HorizontalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
        }

        private void DoubleHorizontalAttack()
        {
            Instantiate(HorizontalLeftAttackPrefab, RightClawPosition.position, transform.rotation);
            Instantiate(HorizontalRightAttackPrefab, LeftClawPosition.position, transform.rotation);
        }
    }
}