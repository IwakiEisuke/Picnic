using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Health : MonoBehaviour
{
    [SerializeField] UnitStats stats;
    [SerializeField] GameObject dieObj;

    public UnityEvent OnDied;

    int _currentHealth;

    public float HealthRatio => 1f * _currentHealth / stats.MaxHealth;

    public void Start()
    {
        _currentHealth = stats.MaxHealth;
    }

    public void TakeDamage(AttackReceiveInfo info)
    {
        _currentHealth -= info.damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDied?.Invoke();
        Destroy(gameObject);

        if (dieObj != null)
        {
            Instantiate(dieObj, transform.position, transform.rotation);
        }
    }
}