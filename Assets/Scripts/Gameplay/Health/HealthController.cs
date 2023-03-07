using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public abstract class HealthController : MonoBehaviour
    {
        
        [SerializeField] protected int maxHealth = 5;
        [SerializeField] protected float invincibilityCooldown = 0.5f;
        [SerializeField] protected float timeToDestroyObject = 0.5f;
        private bool _canTakeDamage;
        private int _health;

        private void Start()
        {
            InitializeHealth();
            WhenInitializeHealth();
        }

        protected virtual void InitializeHealth()
        {
            _canTakeDamage = true;
            _health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0) return;
            if (!_canTakeDamage) return;
            
            _health -= damage;
            
            WhenTakeDamage(damage);
            CheckDeathAndKill();
            StartCoroutine(CountInvincibilityCooldown());
        }
        
        public virtual void ApplyHeal(int heal)
        {
            WhenApplyHeal(heal);
            _health = Mathf.Clamp(_health + heal, 0, maxHealth);
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

        protected virtual void Kill()
        {
            WhenKill();    
            Destroy(gameObject, timeToDestroyObject);
        }

        protected abstract void WhenInitializeHealth();
        protected abstract void WhenKill();
        protected abstract void WhenTakeDamage(int damage);
        protected abstract void WhenApplyHeal(int heal);

    }
}