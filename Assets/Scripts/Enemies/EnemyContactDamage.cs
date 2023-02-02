using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamage = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        //Hit player
    }
}
