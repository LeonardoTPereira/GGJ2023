using System;
using System.Collections;
using System.Collections.Generic;
using Scenario.Collectables;
using UnityEngine;

namespace Player
{
    
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

    
}