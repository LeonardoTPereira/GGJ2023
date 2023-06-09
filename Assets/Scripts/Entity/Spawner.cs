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
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, new Vector3(3, 3, 1));
        }
    }
}