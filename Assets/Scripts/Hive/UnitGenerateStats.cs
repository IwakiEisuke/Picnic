using UnityEngine;

[CreateAssetMenu]
public class UnitGenerateStats : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] int maxCount;
    [SerializeField] float timeToGenerate;

    public GameObject Prefab => prefab;
    public int MaxCount => maxCount;
    public float TimeToGenerate => timeToGenerate;
}
