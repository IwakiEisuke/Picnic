using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 旧ウェーブシステム
/// </summary>
public class WaveManager : MonoBehaviour
{
    [SerializeField] WaveSO[] waves;
    [SerializeField] EnemySpawnerManager enemySpawnerManager;

    public UnityEvent OnStartWave;
    public UnityEvent OnEndWave;

    int waveCount = 0;

    [ContextMenu("Start Wave")]
    public void StartWave()
    {
        waveCount++;

        if (waveCount <= waves.Length)
        {
            enemySpawnerManager.SetSpawn(waves[waveCount - 1]);
        }

        OnStartWave.Invoke();
    }

    public void EndWave()
    {
        OnEndWave.Invoke();
    }
}
