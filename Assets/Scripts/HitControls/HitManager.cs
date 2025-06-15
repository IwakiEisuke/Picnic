using System;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    [SerializeField] bool debugMode;
    [SerializeField] Health health;

    readonly DamageHistoryManager damageHistoryManager = new();

    public event Action<AttackReceiveInfo> OnDamaged;
    public event Action<AttackReceiveInfo> OnAttacked;

    public void ReceiveHit(AttackReceiveInfo info)
    {
        if (health == null)
        {
            Debug.LogWarning("Health component is not assigned in HitManager.");
            return;
        }

        if (damageHistoryManager.CanHit(info))
        {
            health.TakeDamage(info);
            OnDamaged?.Invoke(info);
            info.attacker.GetComponent<HitManager>()?.OnAttacked?.Invoke(info);
        }
    }

    private void Update()
    {
        damageHistoryManager.Update();
    }

    private void Start()
    {
        OnDamaged += info =>
        {
            if (debugMode) Debug.Log($"Received damage: {info.damage} from {info.attacker.name} with ID {info.id}");
        };
        OnAttacked += info =>
        {
            if (debugMode) Debug.Log($"Attacked: {info.damage} damage to {gameObject.name} from {info.attacker.name} with ID {info.id}");
        };
    }
}

public class AttackReceiveInfo
{
    public int damage;
    public int id;
    public float invincibleTime;
    public Transform attacker;

    public AttackReceiveInfo(AttackData attackData, Transform attacker)
    {
        damage = attackData.damage;
        id = attackData.id;
        invincibleTime = attackData.invincibleTime;
        this.attacker = attacker;
    }
}

public class DamageHistoryManager
{
    List<DamageHistory> damageHistories = new();

    public class DamageHistory
    {
        public int id;
        public float duration;
        public Transform attacker;

        public DamageHistory(AttackReceiveInfo info)
        {
            id = info.id;
            duration = info.invincibleTime;
            attacker = info.attacker;
        }
    }

    private void AddHistory(AttackReceiveInfo info)
    {
        if (info.invincibleTime > 0)
        {
            damageHistories.Add(new DamageHistory(info));
        }
    }

    public bool CanHit(AttackReceiveInfo info)
    {
        for (int i = 0; i < damageHistories.Count; i++)
        {
            if (damageHistories[i].id == info.id && damageHistories[i].attacker == info.attacker)
            {
                return false;
            }
        }

        AddHistory(info);
        return true;
    }

    public void Update()
    {
        var dt = Time.deltaTime;
        for (int i = 0; i < damageHistories.Count; i++)
        {
            damageHistories[i].duration -= dt;
            if (damageHistories[i].duration <= 0)
            {
                damageHistories.RemoveAt(i);
            }
        }
    }
}