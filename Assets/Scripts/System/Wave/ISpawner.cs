using System.Collections;

public interface ISpawner
{
    IEnumerator Spawn(WaveEnemyData waveEnemyData);
}
