using System.Collections;
using UnityEngine;

namespace Gameplay.Bullets
{
    public abstract class AbstractBulletMovement : ScriptableObject
    {
        public abstract IEnumerator Move(Vector2 speed, BulletController bulletController);
    }
}