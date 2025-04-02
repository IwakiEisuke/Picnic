using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] UnitStats stats;

    int _maxHealth;
    int _currentHealth;

    public float HealthRatio => 1f * _currentHealth / _maxHealth;

    private void Awake()
    {
        _maxHealth = stats.MaxHealth;
        _currentHealth = stats.MaxHealth;
    }
}
