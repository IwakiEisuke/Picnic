using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/WaveData")]
public class WaveData : ScriptableObject
{
    public StageData parentStage;
    public List<EnemySpawnEvent> spawnEvents;

    public List<SpawnEntityData> EntitiesData => parentStage.entitiesData;
    public List<SpawnPointBase> SpawnPoints => parentStage.spawnPoints;
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
public class SpawnPointBase
{
    public Vector3 position;
    public Quaternion rotation;

    public void Spawn(RuntimeSpawnEvent spawnEvent)
    {
        var spawnEntity = spawnEvent.entityData;

        for (int i = 0; i < spawnEvent.spawnCount; i++)
        {
            GetSpawnPosition(out var spawnPos, out var spawnRot);
            GameObject.Instantiate(spawnEntity.entityPrefab, spawnPos, spawnRot);
            //Debug.Log($"Spawning {spawnEntity.name} at {spawnPos} with rotation {spawnRot}");
        }
    }

    protected virtual void GetSpawnPosition(out Vector3 spawnPosition, out Quaternion spawnRotation)
    {
        spawnPosition = position;
        spawnRotation = rotation;
    }
}

public class CircleSpawnPoint : SpawnPointBase
{
    public float radius = 1f;

    protected override void GetSpawnPosition(out Vector3 spawnPosition, out Quaternion spawnRotation)
    {
        var randomPos = UnityEngine.Random.insideUnitCircle * radius;
        spawnPosition = position + new Vector3(randomPos.x, 0, randomPos.y);
        spawnRotation = rotation;
    }
}
