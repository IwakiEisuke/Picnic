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

    public int MaxHealth => maxHealth;
    public float Speed => speed;
    public int Atk => atk;
    public float AttackInterval => attackInterval;
    public float AttackRadius => attackRadius;
    public float KnockBack => knockBack;
}
