using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class LineRangedAttack : Attack
    {
        
        [SerializeField] private float initialForce = 50f;

        protected override void Initialize()
        {
            //Do nothing
        }
        
        protected override void Shoot()
        {
            var currentBullet = Instantiate(bullet, mouth.position, transform.rotation);
            currentBullet.GetComponent<Rigidbody2D>().AddForce(transform.right*initialForce, ForceMode2D.Impulse);
            AudioManager.Instance.PlaySFX("Tiro_Inimigo_Curto");
        }

    }
    
}
