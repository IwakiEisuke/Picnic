using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class WaveSO : ScriptableObject
{
    public WaveEnemyData[] waveEnemyData;
}

[System.Serializable]
public struct WaveEnemyData
{
    public GameObject prefab;
    public int count;
    public float startTime;
    public float spawnInterval;
    public int spawnPosition;
}