using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTwoWay : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Transform currentPoint;
    
    private Rigidbody2D enemyRb;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentPoint = pointA;
    }

    private void FixedUpdate()
    {
        enemyRb.velocity = transform.right * speed + new Vector3(0, enemyRb.velocity.y, 0);
        
        if (CheckFlip())
        {
            Flip();
        }

        if (CheckPointReached())
        {
            ChangePoint();
        }
    }

    private bool CheckFlip()
    {
        var currentTransform = transform;
        return (currentPoint.position - currentTransform.position).x * (currentTransform.forward).x < 0;
    } 
    
    private void Flip()
    {
        transform.Rotate(Vector3.up, 180);
    }

    private bool CheckPointReached()
    {
        return Vector3.Distance(transform.position, currentPoint.position) < 0.1f;
    }

    private void ChangePoint()
    {
        if (currentPoint == pointA)
            currentPoint = pointB;
        
        else if (currentPoint == pointB)
            currentPoint = pointA;
    }
}
