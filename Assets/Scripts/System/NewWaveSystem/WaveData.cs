using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/WaveData")]
public class WaveData : ScriptableObject
{
    public StageData parentStage;
    public List<EnemySpawnEvent> spawnEvents;

    public List<GameObject> EnemyPrefabs => parentStage.enemyPrefabs;
    public List<SpawnPoint> SpawnPoints => parentStage.spawnPoints;
}

[Serializable]
public class EnemySpawnEvent
{
    public float time;
    public int enemyIndex;
    public int spawnPointIndex;

    [Min(1)] public int spawnCountPerBatch = 1;
    [Min(1)] public int repeatCount = 1;
    [Min(0)] public float repeatInterval;

    public string id;

    public int TotalSpawnCount => spawnCountPerBatch * repeatCount;

    public EnemySpawnEvent()
    {
        id = Guid.NewGuid().ToString();
    }
}

[Serializable]
public class SpawnPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public SpawnPoint(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
