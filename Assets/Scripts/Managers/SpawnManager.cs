using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    private int CurrentSpawnIndex { get; set; }
    private List<Transform> _spawnList = new List<Transform>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        foreach (Transform transf in transform.GetComponentsInChildren<Transform>())
        {
            _spawnList.Add(transf);
        }

        CurrentSpawnIndex = 1;
    }

    public void UpdateSpawnPoint()
    {
        if (CurrentSpawnIndex < _spawnList.Count - 1)
        {
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
