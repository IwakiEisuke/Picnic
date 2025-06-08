using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitBase : MonoBehaviour, IDamageable, IHealth, IUnit
{
    [SerializeField] protected UnitStats stats;
    [SerializeField] protected Health health;

    [SerializeField] public LayerMask opponentLayer;
    [SerializeField] public string destinationTag;

    [SerializeField] protected EvolutionTree evolutionTree;

    protected Rigidbody _rb;
    protected NavMeshAgent _agent;
    protected Collider[] _hits = new Collider[1];
    protected EntityObserver observer;

    public UnitStats Stats { get { return stats; } }

    public Health Health => health;
    public NavMeshAgent Agent => _agent;

    public EvolutionTree EvolutionTree => evolutionTree;

    public event Action Destroyed;

    protected void Awake()
    {
        InitializeUnitBase();
    }

    protected void InitializeUnitBase()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = Stats.Speed;
        health.Init(transform);
        health.OnDied.AddListener(Die);
        observer = new EntityObserver(stats.name);
        observer.Register();
    }

    public void Die()
    {
        observer.Remove();
        StopAllCoroutines();
    }

    public void TakeDamage(UnitStats other)
    {
        _agent.velocity = transform.position.normalized * other.KnockBack;
        health.TakeDamage(other);
    }

    public void Evolve(UnitBase prefab)
    {
        var evolved = Instantiate(prefab, transform.position, transform.rotation);
        
        // 状態異常の引き継ぎ

        // 進化状態の引き継ぎ
        if (evolutionTree != null)
        {
            evolved.evolutionTree.Copy(evolutionTree);
        }


        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (stats)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, stats.AttackRadius);
        }
    }

    private void OnDestroy()
    {
        Destroyed?.Invoke();
    }
}


public interface IUnit
{
    public event Action Destroyed;
}