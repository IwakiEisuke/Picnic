using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Health : IDamageable
{
    [SerializeField] UnitStats stats;
    [SerializeField] GameObject dieObj;

    public UnityEvent OnDied;

    int _currentHealth;
    Transform _parent;

    public float HealthRatio => 1f * _currentHealth / stats.MaxHealth;

    public void Init(Transform parent)
    {
        _parent = parent;
        _currentHealth = stats.MaxHealth;
    }

    public void TakeDamage(UnitStats other)
    {
        _currentHealth -= other.Atk;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDied?.Invoke();
        GameObject.Destroy(_parent.gameObject);

        if (dieObj != null)
        {
            GameObject.Instantiate(dieObj, _parent.position, _parent.rotation);
        }
    }
}

public interface IHealth
{
    public Health Health { get; }
}