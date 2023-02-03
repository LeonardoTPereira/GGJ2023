using System;
using System.Collections;
using System.Collections.Generic;
using Scenario.Collectables;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var collectable = col.gameObject.GetComponent<Collectable>();

        if (collectable == null)
            return;
        
        collectable.BeCollectedBy(player);
    }
}
