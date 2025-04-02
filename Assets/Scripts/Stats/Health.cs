using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] UnitStats stats;

    public float HealthRatio => 1f * _currentHealth / _maxHealth;

    public event Action OnDied;

    int _maxHealth;
    int _currentHealth;

    bool _isDead;

    public void TakeDamage(UnitStats other)
    {
        _currentHealth -= other.Atk;

        if (_currentHealth <= 0)
        {
            OnDied?.Invoke();
            _isDead = true;
        }
    }

    private void OnDestroy()
    {
        if (!_isDead) OnDied?.Invoke();
    }

    private void Awake()
    {
        _maxHealth = stats.MaxHealth;
        _currentHealth = stats.MaxHealth;
    }
}
