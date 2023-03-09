using UnityEngine;

namespace Gameplay.Bullets
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet", order = 0)]
    public class BulletSO : ScriptableObject
    {
        [SerializeField] private string bulletName;
        [SerializeField] private float xSpeed;
        [SerializeField] private float ySpeed;
        [SerializeField] private float cooldown;
        [SerializeField] private int damage;
        [SerializeField] private AbstractBulletMovement bulletMovement;

        public string Name
        {
            get => bulletName;
            set => bulletName = value;
        }

        public float XSpeed
        {
            get => xSpeed;
            set => xSpeed = value;
        }

        public float YSpeed
        {
            get => ySpeed;
            set => ySpeed = value;
        }

        public float Cooldown
        {
            get => cooldown;
            set => cooldown = value;
        }

        public int Damage
        {
            get => damage;
            set => damage = value;
        }

        public AbstractBulletMovement BulletMovement
        {
            get => bulletMovement;
            set => bulletMovement = value;
        }
    }
}