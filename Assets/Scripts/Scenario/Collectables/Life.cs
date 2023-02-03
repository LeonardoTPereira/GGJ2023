using System.Collections;
using System.Collections.Generic;
using Scenario.Collectables;
using UnityEngine;

public class Life : Collectable
{
    [SerializeField] private LifeData life;
    
    public override void BeCollectedBy(GameObject collector)
    {
        //Call life controller
        Debug.Log("Life");
        Destroy(gameObject);
    }
}
