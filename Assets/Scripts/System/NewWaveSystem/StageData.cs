using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveEditor/StageData")]
public class StageData : ScriptableObject
{
    public List<SpawnPoint> spawnPoints;
    public List<GameObject> enemyPrefabs;
    public List<WaveData> waves = new();
}
