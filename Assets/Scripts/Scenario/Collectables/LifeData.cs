using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario.Collectables
{
    
    [CreateAssetMenu(fileName = "Life", menuName = "ScriptableObjects/Collectable/Life")]
    public class LifeData : ScriptableObject
    {
        public int health;
    }

    
}
