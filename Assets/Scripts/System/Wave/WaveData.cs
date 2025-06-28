using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/WaveData")]
public class WaveData : ScriptableObject
{
    public List<GameObject> enemyPrefabs;
    public List<SpawnPoint> spawnPoints;
    public List<EnemySpawnEvent> spawnEvents;
}

[Serializable]
public class EnemySpawnEvent
{
    public float time;
    public int enemyIndex;
    public int spawnPointIndex;
}

[Serializable]
public class SpawnPoint
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public SpawnPoint(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
