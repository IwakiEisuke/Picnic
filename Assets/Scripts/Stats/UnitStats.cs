using System;
using UnityEngine;

[CreateAssetMenu]
public class UnitStats : ScriptableObject
{
    [SerializeField] int maxHealth;
    [SerializeField] float speed;
    [SerializeField] int atk;
    [SerializeField] float attackInterval;
    [SerializeField] float attackRadius;
    [SerializeField] float knockBack;

    public bool isSortie;

    public void Init()
    {
        isSortie = false;
    }

    public int MaxHealth => maxHealth;
    public float Speed => speed;
    public int Atk => atk;
    public float AttackInterval => attackInterval;
    public float AttackRadius => attackRadius;
    public float KnockBack => knockBack;
}

public class UnitGameStatus
{
    public UnitBase owner;
    public int maxHealth;
    public float speed;
    public int atk;
    public float attackInterval;
    public float attackRadius;
    public float knockBack;

    public UnitGameStatus(UnitStats stats, UnitBase owner)
    {
        maxHealth = stats.MaxHealth;
        speed = stats.Speed;
        atk = stats.Atk;
        attackInterval = stats.AttackInterval;
        attackRadius = stats.AttackRadius;
        knockBack = stats.KnockBack;
        this.owner = owner;
    }
}