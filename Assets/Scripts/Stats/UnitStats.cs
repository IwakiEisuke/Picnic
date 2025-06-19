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

    [SerializeField] float damageReflect;
    [SerializeField] float resistance;

    [SerializeField] ActionBase[] actions;

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

    public float DamageReflect => damageReflect;
    public float Resistance => resistance;
    public ActionBase[] Actions => actions;
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

    public float damageReflect;
    public float resistance;

    public UnitGameStatus(UnitStats stats, UnitBase owner)
    {
        maxHealth = stats.MaxHealth;
        speed = stats.Speed;
        atk = stats.Atk;
        attackInterval = stats.AttackInterval;
        attackRadius = stats.AttackRadius;
        knockBack = stats.KnockBack;

        damageReflect = stats.DamageReflect;
        resistance = stats.Resistance;

        this.owner = owner;
    }

    public void Replace(UnitGameStatus stats)
    {
        maxHealth = stats.maxHealth;
        speed = stats.speed;
        atk = stats.atk;
        attackInterval = stats.attackInterval;
        attackRadius = stats.attackRadius;
        knockBack = stats.knockBack;
        damageReflect = stats.damageReflect;
        resistance = stats.resistance;
    }
}