using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public IEnumerator Spawn(WaveEnemyData waveEnemyData)
    {
        for (int i = 0; i < waveEnemyData.count; i++)
        {
            Instantiate(waveEnemyData.enemy, transform.position, transform.rotation);
            yield return new WaitForSeconds(waveEnemyData.spawnInterval);
        }
    }
}