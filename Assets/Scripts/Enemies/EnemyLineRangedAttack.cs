using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float initialForce = 50f;
    [SerializeField] private float repeatRate = 2f;
    
    private void Start()
    {
        InvokeRepeating("Shoot", 0.5f, repeatRate);
    }
    
    private void Shoot()
    {
        var currentBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody2D>().AddForce(transform.right*initialForce, ForceMode2D.Impulse);
    }

}
