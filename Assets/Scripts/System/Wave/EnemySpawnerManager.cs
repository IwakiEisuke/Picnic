using System.Collections;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] EnemySpawner[] spawners;

    public int SpawnersCount => spawners.Length;

    public EnemySpawner GetSpawner(int i)
    {
        return spawners[i];
    }

    public void SetSpawn(WaveSO wave)
    {
        foreach (var waveEnemyData in wave.waveEnemyData)
        {
            StartCoroutine(Spawn(waveEnemyData));
        }
    }

    public IEnumerator Spawn(WaveEnemyData waveEnemyData)
    {
        yield return new WaitForSeconds(waveEnemyData.startTime);

        if (waveEnemyData.spawnPosition >= 0)
        {
            StartCoroutine(spawners[waveEnemyData.spawnPosition].Spawn(waveEnemyData));
        }
        else
        {
            var divided = waveEnemyData;
            divided.count /= SpawnersCount;

            foreach (var spawner in spawners)
            {
                StartCoroutine(spawner.Spawn(divided));
            }
        }
    }
}
