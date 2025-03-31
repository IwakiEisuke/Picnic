using UnityEngine;

[CreateAssetMenu]
public class UnitStats : ScriptableObject
{
    [SerializeField] int maxHealth;
    [SerializeField] float speed;
    [SerializeField] int atk;
    [SerializeField] float attackInterval;
    [SerializeField] float attackRadius;

    public int MaxHealth => maxHealth;
    public float Speed => speed;
    public int Atk => atk;
    public float AttackInterval => attackInterval;
    public float AttackRadius => attackRadius;
}
