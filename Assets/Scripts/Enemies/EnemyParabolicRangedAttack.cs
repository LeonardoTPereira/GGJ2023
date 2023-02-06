using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyParabolicRangedAttack : MonoBehaviour
{
    [SerializeField] private float gravity;
    private Transform target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform mouth;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        
        InvokeRepeating("AnimateShoot", 0f, fireRate);
        InvokeRepeating("Shoot", 0.5f, fireRate);
    }

    private void AnimateShoot()
    {
        animator.SetTrigger("Attack");
    }

    private void Shoot()
    {
        if (CheckFlip())
            Flip();

        var currentBullet = Instantiate(bullet, mouth.position, transform.rotation);

        var angle = GetCurrentLaunchAngle();
        var speed = GetCurrentLaunchSpeed(angle);
        
        var bulletVelocity = (new Vector2(Mathf.Cos(angle) * transform.right.x, Mathf.Sin(angle))) * speed ;

        currentBullet.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        
    }
    
    private bool CheckFlip()
    {
        return Vector2.Dot(target.position - transform.position, transform.right) < 0;
    } 
    
    private void Flip()
    {
        transform.Rotate(Vector3.up, 180);
        var currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x, currentScale.y, -currentScale.z);
    }

    private float GetCurrentLaunchAngle()
    {
        var relativePosition = target.position - transform.position;
        var a = relativePosition.x;
        var b = relativePosition.y;

        if (a < 0)
            a = -a;

        var minAngle = Mathf.Atan2(b, a);

        if (minAngle >= Mathf.Deg2Rad * 80f)
            return Mathf.Deg2Rad * 90f;
        
        if (minAngle >= Mathf.Deg2Rad * 60f)
            return Mathf.Deg2Rad * 80f;
        
        if (minAngle >= Mathf.Deg2Rad * 45f)
            return Mathf.Deg2Rad * 60f;
        
        return Mathf.Deg2Rad * 45f;

    }
    
    private float GetCurrentLaunchSpeed(float angle)
    {
        var relativePosition = target.position - transform.position;
        var a = relativePosition.x;
        var b = relativePosition.y;

        if (a < 0)
            a = -a;

        if (a * Mathf.Tan(Mathf.Deg2Rad * 80f) < b)
            return 100f;    
        
        return Mathf.Sqrt(gravity * a * a / ( 2* Mathf.Pow(Mathf.Cos(angle), 2) * (a * Mathf.Tan(angle) - b)));
    }
    
}
