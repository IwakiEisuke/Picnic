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
}
