using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class MoveForward : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
    
        private Rigidbody2D enemyRb;

        private void Awake()
        {
            enemyRb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            enemyRb.velocity = transform.right * speed;
        }

    }
    
}
