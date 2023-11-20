using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    private int CurrentSpawnIndex { get; set; }
    public Collider2D CameraBound { get => _cameraBound; private set => _cameraBound = value; }

    private readonly List<Transform> _spawnList = new();
    private Collider2D _cameraBound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        foreach (Transform transf in transform.GetComponentsInChildren<Transform>())
        {
            _spawnList.Add(transf);
        }

        CurrentSpawnIndex = 1;
        _cameraBound = null;
    }

    public void UpdateSpawnPoint(Collider2D newCameraBound)
    {
        if (CurrentSpawnIndex < _spawnList.Count - 1)
        {
            CameraBound = newCameraBound;
            CurrentSpawnIndex++;
        }
    }

    public Transform GetSpawnPoint()
    {
        return _spawnList[CurrentSpawnIndex];
    }

    public void ResetSpawnPoint()
    {
        CurrentSpawnIndex = 1;
    }
}