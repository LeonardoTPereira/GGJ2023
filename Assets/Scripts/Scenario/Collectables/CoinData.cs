using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario.Collectables
{
    
    [CreateAssetMenu(fileName = "Coin", menuName = "ScriptableObjects/Collectable/Coin")]
    public class CoinData : ScriptableObject
    {
        public int amount;
    }
    
}