using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Health : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;
    [SerializeField] GameObject dieObj;

    int _currentHealth;

    public UnityEvent OnDied;

    UnitGameStatus Status => unitBase.Status;
    public float HealthRatio => 1f * _currentHealth / Status.maxHealth;

    private void Start()
    {
        _currentHealth = Status.maxHealth;
    }

    public DamageResponse ApplyDamage(AttackReceiveInfo info)
    {
        // 受け取ったダメージ量を計算。ステータスの耐性を考慮して、最終的なダメージ量を決定する。完全な耐性が無い限り1ダメージは与えるようにするため切り上げ
        var damage = Mathf.CeilToInt(info.damage * Mathf.Max(0, (1 - Status.resistance)));
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

    public void Heal(float value)
    {
        _currentHealth += Mathf.CeilToInt(value);
        if (_currentHealth > Status.maxHealth)
        {
            _currentHealth = Status.maxHealth;
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