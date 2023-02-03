using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario.Collectables
{
    public abstract class Collectable : MonoBehaviour
    {
        public abstract void BeCollectedBy(GameObject collector);
    }

}
