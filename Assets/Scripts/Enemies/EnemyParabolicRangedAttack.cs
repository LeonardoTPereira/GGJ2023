using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyParabolicRangedAttack : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate;

    private void Start()
    {
        InvokeRepeating("Shoot", 0.5f, fireRate);
    }

    private void Shoot()
    {
        if (CheckFlip())
            Flip();

        var currentBullet = Instantiate(bullet, transform.position, transform.rotation);
        currentBullet.GetComponent<Rigidbody2D>().velocity =
            (new Vector2(Mathf.Sqrt(2) / 2 * transform.right.x, Mathf.Sqrt(2) / 2)) * GetCurrentLaunchSpeed();
    }
    
    private bool CheckFlip()
    {
        return Vector2.Dot(target.position - transform.position, transform.right) < 0;
    } 
    
    private void Flip()
    {
        transform.Rotate(Vector3.up, 180);
    }
    
    private float GetCurrentLaunchSpeed()
    {
        var relativePosition = target.position - transform.position;
        var a = relativePosition.x;
        var b = relativePosition.y;

        if (a < 0)
            a = -a;

        if (a < b)
            return 0f;
        
        return Mathf.Sqrt(gravity * a * a / (a - b));
    }
    
}
