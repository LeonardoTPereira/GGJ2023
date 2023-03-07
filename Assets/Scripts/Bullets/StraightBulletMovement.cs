using System.Collections;
using UnityEngine;

namespace Gameplay.Bullets
{
    [CreateAssetMenu(fileName = "StraightBulletMovement", menuName = "Scriptable Objects/Straight Bullet", order = 0)]
    public class StraightBulletMovement : AbstractBulletMovement
    {
        public override IEnumerator Move(Vector2 speed, BulletController bulletController)
        {
            bulletController.RigidBody.velocity = speed;
            yield return null;
        }
    }
}