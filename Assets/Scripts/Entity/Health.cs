using System.Collections;
using UnityEngine;

namespace Entity
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] protected int maxHealth = 5;
        [SerializeField] protected float invincibilityCooldown = 0.5f;
        [SerializeField] protected float timeToDestroyObject;
        [SerializeField] protected int _health;
        protected bool _isInMovementInvincibility = false;

        private bool _canTakeDamage;

        protected virtual void Start()
        {
            InitializeHealth();
            WhenInitializeHealth();
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0) return;
            if (!_canTakeDamage || _isInMovementInvincibility) return;

            _health -= damage;

            WhenTakeDamage(damage);
            CheckDeathAndKill();
            StartCoroutine(CountInvincibilityCooldown());
        }

        public void ApplyHeal(int heal)
        {
            WhenApplyHeal(heal);
            _health = Mathf.Clamp(_health + heal, 0, maxHealth);
        }

        private void InitializeHealth()
        {
            _canTakeDamage = true;
            _health = maxHealth;
        }

        private void CheckDeathAndKill()
        {
            if (_health > 0) return;
            Kill();
        }

        private IEnumerator CountInvincibilityCooldown()
        {
            _canTakeDamage = false;
            yield return new WaitForSeconds(invincibilityCooldown);
            _canTakeDamage = true;
        }

        public IEnumerator CountMovementInvincibilityCooldown(float cooldown)
        {
            _isInMovementInvincibility = true;
            yield return new WaitForSeconds(cooldown);
            _isInMovementInvincibility = false;
        }

        private void Kill()
        {
            WhenKill();
        }

        protected abstract void WhenInitializeHealth();

        protected abstract void WhenKill();

        protected abstract void WhenTakeDamage(int damage);

        protected abstract void WhenApplyHeal(int heal);
    }
}