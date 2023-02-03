using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    FirstWeapon
}

namespace Scenario.Collectables
{

    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Collectable/Weapon")]
    public class WeaponData : ScriptableObject
    {
        public WeaponType type;
    }    
}

