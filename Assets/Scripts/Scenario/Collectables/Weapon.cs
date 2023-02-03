using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario.Collectables
{
    public class Weapon : Collectable
    {

        [SerializeField] private WeaponData data;
        
        public override void BeCollectedBy(GameObject collector)
        {
            //Call collector's weapon controller.
            Debug.Log("Weapon");
            Destroy(gameObject);
        }
    }    
}

