using System;

namespace Gameplay.Bullets
{
    public delegate void BulletHitEventHandler(object sender, BulletHitEventArgs eventArgs);

    public class BulletHitEventArgs : EventArgs
    {
        public BulletSO Bullet { get; set; }

        public BulletHitEventArgs(BulletSO bullet)
        {
            Bullet = bullet;
        }
    }
}