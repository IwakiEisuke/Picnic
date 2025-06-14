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
    Transform _parent;

    public float HealthRatio => 1f * _currentHealth / stats.MaxHealth;

    public void Init(Transform parent)
    {
        _parent = parent;
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
        GameObject.Destroy(_parent.gameObject);

        if (dieObj != null)
        {
            GameObject.Instantiate(dieObj, _parent.position, _parent.rotation);
        }
    }
}