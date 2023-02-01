using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveLeft : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    private Rigidbody2D enemyRb;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        enemyRb.velocity = transform.forward * speed;
    }

}
