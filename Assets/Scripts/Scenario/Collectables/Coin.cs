using System.Collections;
using System.Collections.Generic;
using Scenario.Collectables;
using UnityEngine;

public class Coin : Collectable
{

    [SerializeField] private CoinData coin;
    
    public override void BeCollectedBy(GameObject collector)
    {
        //Call coin controller
    }
}
