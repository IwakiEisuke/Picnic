using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] UnitStats stats;

    public float HealthRatio => 1f * _currentHealth / _maxHealth;

    public event Action OnDied;
    public event Action OnDestroyEvent;

    int _maxHealth;
    int _currentHealth;

    public void TakeDamage(UnitStats other)
    {
        _currentHealth -= other.Atk;

        if (_currentHealth <= 0)
        {
            OnDied?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }

    private void Awake()
    {
        _maxHealth = stats.MaxHealth;
        _currentHealth = stats.MaxHealth;
    }
}
