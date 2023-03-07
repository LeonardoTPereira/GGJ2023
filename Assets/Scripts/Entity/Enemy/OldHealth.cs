using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class OldHealth : MonoBehaviour
    {
        [SerializeField] protected int maxHealth = 5;
        protected int currentHealth;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(int amount)
        {
            if (amount < 0)
                return;

            currentHealth -= amount;

            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            //Do something
            Destroy(gameObject);
        }
    }
    
}
