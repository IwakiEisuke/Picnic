using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] UnitStats stats;
    [SerializeField] GameObject dieObj;

    public float HealthRatio => 1f * _currentHealth / _maxHealth;

    public UnityEvent OnDied;
    public event Action OnDestroyEvent;

    int _maxHealth;
    int _currentHealth;

    public void TakeDamage(UnitStats other)
    {
        _currentHealth -= other.Atk;

        if (_currentHealth <= 0)
        {
            if (dieObj != null) Instantiate(dieObj, transform.position, transform.rotation);
            Destroy(gameObject);
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
