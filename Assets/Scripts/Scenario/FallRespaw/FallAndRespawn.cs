using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndRespawn : MonoBehaviour
{
    private static GameObject _player;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private int _damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _player.transform.position = _spawnPos.position;
            col.gameObject.GetComponent<Player.Health>().TakeDamage(_damage);
        }
    }
}
