using System.Collections;
using UnityEngine;

public class CircleEnemySpawner : EnemySpawner, ISpawner
{
    [SerializeField] float radius;

    public override IEnumerator Spawn(WaveEnemyData waveEnemyData)
    {
        for (int i = 0; i < waveEnemyData.count; i++)
        {
            var theta = Random.value * Mathf.PI * 2;
            var spawnPosition = transform.position + new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)) * radius;

            Instantiate(waveEnemyData.prefab, spawnPosition, Quaternion.LookRotation(-spawnPosition, Vector3.up));

            yield return new WaitForSeconds(waveEnemyData.spawnInterval);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
