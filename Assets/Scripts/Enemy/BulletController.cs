using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    
    public class BulletController : MonoBehaviour
    {

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(30);
        
            DestroyBullet();                
        }
    
        private void DestroyBullet()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                var health = col.gameObject.GetComponent<PlayerHealth>();
                health.TakeDamage(1);
                DestroyBullet();
            }
        
            else if (col.gameObject.CompareTag("Block"))
            {
                DestroyBullet();
            }
        }
    }

    
}
