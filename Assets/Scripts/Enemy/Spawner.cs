using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject entity;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
            Spawn();
    }

    private void Spawn()
    {
        Instantiate(entity, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
