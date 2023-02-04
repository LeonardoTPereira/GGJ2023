using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float initialForce = 50f;
    [SerializeField] private float repeatRate = 2f;
    [SerializeField] private Animator animator;
    
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
        var currentBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody2D>().AddForce(transform.right*initialForce, ForceMode2D.Impulse);
    }

}
