using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class UnitGenerateStats : ScriptableObject
{
    [SerializeField] UnitStats target;
    [SerializeField] GameObject prefab;
    [SerializeField] int maxCount;
    [SerializeField] float timeToGenerate;

    public int exists;
    public int outside;

    public void Init()
    {
        exists = 0;
        outside = 0;
    }

    public UnitStats Target { get { return target; } }
    public GameObject Prefab { get { return prefab; } }
    public int MaxCount { get { return maxCount; } }
    public float TimeToGenerate { get { return timeToGenerate; } }

    readonly Timer timer = new();
    public float ProgressRatio => timer.ProgressRatio;

    bool generate;

    public IEnumerator Generate()
    {
        timer.Set(timeToGenerate);
        timer.TimeUp += () => generate = true;

        while (true)
        {
            yield return null;

            if (generate)
            {
                if (exists < maxCount)
                {
                    exists += 1;
                    generate = false;
                    timer.Set(timeToGenerate);
                }
            }
        }
    }
}
