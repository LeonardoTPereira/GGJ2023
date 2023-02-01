using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTwoWay : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float maxHorizontalDistance = 1f;
    [SerializeField] private float maxVerticalDistance = 1f;

    private Transform currentPoint;
    
    private Rigidbody2D enemyRb;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        enemyRb.velocity = transform.right * speed + new Vector3(0, enemyRb.velocity.y, 0);
        
        if (CheckFlip())
        {
            Flip();
        }

    }

    private bool CheckFlip()
    {
        var currentTransform = transform;
        
        return (Physics2D.Raycast(currentTransform.position + currentTransform.right * maxHorizontalDistance,
            -currentTransform.up,
            maxVerticalDistance, mask).collider == null);
    } 
    
    private void Flip()
    {
        transform.Rotate(Vector3.up, 180);
    }

}
