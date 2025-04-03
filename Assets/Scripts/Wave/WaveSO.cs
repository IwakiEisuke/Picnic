using UnityEngine;

[CreateAssetMenu]
public class WaveSO : ScriptableObject
{
    public WaveEnemyData[] waveEnemyData;
}

[System.Serializable]
public struct WaveEnemyData
{
    public GameObject enemy;
    public int count;
    public float startTime;
    public float spawnInterval;
    public int spawnPosition;
}