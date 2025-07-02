using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveEditor/StageData")]
public class StageData : ScriptableObject
{
    public List<SpawnPoint> spawnPoints;
    public List<SpawnEntityData> entitiesData;
    public List<WaveData> waves = new();
}
