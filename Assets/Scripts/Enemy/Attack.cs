using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public abstract class Attack : MonoBehaviour
    {
        
        [SerializeField] protected GameObject bullet;
        [SerializeField] private float fireRate;
        [SerializeField] private Animator animator;
        [SerializeField] protected Transform mouth;
        [SerializeField] private float shootOffSet = 0.5f;

        private TransformController transformController;
        
        private void Start()
        {
            Initialize();

            transformController = GetComponent<TransformController>();
            transformController.LookAt(GameObject.FindWithTag("Player").transform);
        
            InvokeRepeating("AnimateShoot", 0f, fireRate);
            InvokeRepeating("Shoot", shootOffSet, fireRate);
        }

        protected abstract void Initialize();

        private void AnimateShoot()
        {
            animator.SetTrigger("Attack");
        }

        protected abstract void Shoot();

    }

    
}