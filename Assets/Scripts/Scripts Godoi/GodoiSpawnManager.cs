using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodoiSpawnManager : MonoBehaviour
{
    public static GodoiSpawnManager instance;

    GodoiSpawnPoint[] spawnPoints;

    private void Awake()
    {
        instance = this;
        spawnPoints = GetComponentsInChildren<GodoiSpawnPoint>();
    }
    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
