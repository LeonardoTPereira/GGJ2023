using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    FirstWeapon
}

namespace Scenario.Collectables
{

    public class WeaponData : ScriptableObject
    {
        public WeaponType type;
    }    
}

