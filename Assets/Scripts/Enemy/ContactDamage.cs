using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class ContactDamage : MonoBehaviour
    {
        [SerializeField] private int contactDamage = 1;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerHealth>().TakeDamage(contactDamage);
            }
        }
    }

    
}