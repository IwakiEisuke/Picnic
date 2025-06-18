using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Health : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;
    [SerializeField] GameObject dieObj;

    public UnityEvent OnDied;

    int _currentHealth;

    UnitStats Stats => unitBase.Stats;
    UnitGameStatus Status => unitBase.status;

    public float HealthRatio => 1f * _currentHealth / Stats.MaxHealth;

    public void Start()
    {
        _currentHealth = Stats.MaxHealth;
    }

    public DamageResponse ApplyDamage(AttackReceiveInfo info)
    {
        var damage = (int)(info.damage * Mathf.Max(0, (1 - Status.resistance)));
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }

        // 反射するダメージ量の計算。プレイヤーから見て効果が分かりやすくなるよう、最低１ダメージは与える
        var reflectDamage = Mathf.CeilToInt(damage * Status.damageReflect);

        if (reflectDamage > 0)
        {
            return new DamageResponse(reflectDamage);
        }
        else
        {
            return null;
        }
    }

    public void ApplyRawDamage(int value)
    {
        _currentHealth -= value;

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

    /// <summary>
    /// 攻撃元に返す反射効果を格納するクラス
    /// </summary>
    public class DamageResponse
    {
        public int damage;
        public DamageResponse(int responseDamage)
        {
            this.damage = responseDamage;
        }
    }
}