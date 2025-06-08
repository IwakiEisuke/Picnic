using UnityEngine;

public class Hive : MonoBehaviour, IDamageable, IHealth
{
    [SerializeField] Health health;

    public Health Health => health;

    private void Start()
    {
        health.Init(transform);
    }

    public void TakeDamage(UnitStats other)
    {
        health.TakeDamage(other);
    }
}