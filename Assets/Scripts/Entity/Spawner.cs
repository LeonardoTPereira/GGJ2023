using UnityEngine;

namespace Entity
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject entity;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("MainCamera"))
                Spawn();
        }

        private void Spawn()
        {
            Instantiate(entity, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GetColorBasedOnEnemyType();
            Gizmos.DrawCube(transform.position, new Vector3(3, 3, 1));
        }

        private Color GetColorBasedOnEnemyType()
        {
            return entity.name switch
            {
                "Enemy1" => Color.blue,
                "Enemy2Shield" => new Color32(98, 52, 0, 255),
                "Enemy2" => Color.green,
                "Enemy3" => Color.magenta,
                "Enemy4" => Color.yellow,
                _ => Color.gray,
            };
        }
    }
}