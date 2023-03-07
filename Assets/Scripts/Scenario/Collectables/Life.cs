using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Scenario.Collectables;
using UnityEngine;

public class Life : Collectable
{
    [SerializeField] private LifeData life;
    
    public override void BeCollectedBy(GameObject collector)
    {
        var health = collector.GetComponent<HealthController>();
        health.ApplyHeal(life.health);
        Destroy(gameObject);
    }
}
