using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomController : MonoBehaviour
{
    private CinemachineConfiner2D _confiner;
    [SerializeField] private CompositeCollider2D _roomCompositeColl;
    [SerializeField] private bool _isSpawnPoint;

    void Start()
    {
        var _virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera");
        _confiner = _virtualCamera.GetComponent<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _confiner.m_BoundingShape2D= _roomCompositeColl;

            if (_isSpawnPoint)
            {
                SpawnManager.Instance.UpdateSpawnPoint();
                _isSpawnPoint = false;
            }
        }
    }
}
