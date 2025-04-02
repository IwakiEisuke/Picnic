using System;
using UnityEngine;

[CreateAssetMenu]
public class UnitGenerateStats : ScriptableObject
{
    [SerializeField] UnitStats target;
    [SerializeField] GameObject prefab;
    [SerializeField] int maxCount;
    [SerializeField] float timeToGenerate;

    [NonSerialized] public int exists;
    [NonSerialized] public int outside;

    public UnitStats Target { get { return target; } }
    public GameObject Prefab { get { return prefab; } }
    public int MaxCount { get { return maxCount; } }
    public float TimeToGenerate { get { return timeToGenerate; } }
}
