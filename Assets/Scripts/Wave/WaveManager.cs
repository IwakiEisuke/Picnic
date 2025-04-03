using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] WaveSO[] waves;
    [SerializeField] EnemySpawnerManager enemySpawnerManager;

    int waveCount = 0;

    [ContextMenu("Start Wave")]
    public void StartWave()
    {
        waveCount++;

        if (waveCount <= waves.Length)
        {
            enemySpawnerManager.SetSpawn(waves[waveCount - 1]);
        }
    }
}
