using System.Collections;
using UnityEngine;

namespace Gameplay.Bullets
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private BulletSO bullet;
        [SerializeField] private float timeToDieOutScreen = 0.5f;

        public Rigidbody2D RigidBody { get; set; }

        private void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();

            var bulletXOrientation = transform.right.x/Mathf.Abs(transform.right.x);
            
            StartCoroutine(Bullet.BulletMovement.Move(new Vector2((bulletXOrientation)*Bullet.XSpeed, Bullet.YSpeed), this));
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!CompareTag("PlayerBullet")) return;
            if (!col.gameObject.CompareTag("Enemy")) return;
            col.gameObject.GetComponent<Entity.Health>().TakeDamage(bullet.Damage);
            DestroyBullet();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Block"))
            {
                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            //TODO play sfx with a Coroutine and kill after it finishes
            Destroy(gameObject);
        }

        public BulletSO Bullet
        {
            get => bullet;
            set => bullet = value;
        }

        private IEnumerator CountCooldown()
        {
            yield return new WaitForSeconds(TimeToDieOutScreen);
        }

        public float TimeToDieOutScreen => timeToDieOutScreen;
    }
}