using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
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
