using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class LineRangedAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private float initialForce = 50f;
        [SerializeField] private float repeatRate = 2f;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform mouth;
    
        private void Start()
        {
            InvokeRepeating("AnimateShoot", 0f, repeatRate);
            InvokeRepeating("Shoot", 0.5f, repeatRate);
        }
    
        private void AnimateShoot()
        {
            animator.SetTrigger("Attack");
        }
    
        private void Shoot()
        {
            var currentBullet = Instantiate(bullet, mouth.position, transform.rotation);
            currentBullet.GetComponent<Rigidbody2D>().AddForce(transform.right*initialForce, ForceMode2D.Impulse);
        }

    }
    
}
