using System.Collections.Generic;

/// <summary>
/// Œ»İƒQ[ƒ€ã‚É‘¶İ‚·‚é“G‚Ì”‚ğŠÇ—‚·‚éƒNƒ‰ƒX
/// </summary>
public static class EnemyManager
{
    static Dictionary<string, int> enemyMap;

    public static void AddEnemy(string name)
    {
        if (enemyMap.TryAdd(name, 1))
        {
            enemyMap[name] += 1;
        }
    }

    public static void RemoveEnemy(string name)
    {
        enemyMap[name] -= 1;
    }

    public static int GetAllEnemyCount()
    {
        var count = 0;

        foreach (var c in enemyMap.Values)
        {
            count += c;
        }

        return count;
    }
}
