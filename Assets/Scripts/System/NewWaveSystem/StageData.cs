using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveEditor/StageData")]
public class StageData : ScriptableObject
{
    [SerializeReference] public List<SpawnPointBase> spawnPoints;
    public List<SpawnEntityData> entitiesData;
    public List<WaveData> waves = new();

    [ContextMenu("Add circle spawn point")]
    public void AddCircleSpawnPoint()
    {
        spawnPoints.Add(new CircleSpawnPoint());
    }

    [ContextMenu("Add spawn point")]
    public void AddSpawnPoint()
    {
        spawnPoints.Add(new SpawnPointBase());
    }
}
