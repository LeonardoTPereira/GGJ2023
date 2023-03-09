using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Enemy
{
    
    public class MoveForward : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        private TransformController transformController;

        private Rigidbody2D enemyRb;

        private void Awake()
        {
            transformController = GetComponent<TransformController>();
            enemyRb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            transformController.LookOneTimeAt(GameObject.FindWithTag("Player").transform);
        }

        private void FixedUpdate()
        {
            enemyRb.velocity = transform.right * speed;
        }

    }
    
}
