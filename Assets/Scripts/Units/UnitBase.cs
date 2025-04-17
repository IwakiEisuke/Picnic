using UnityEngine;
using UnityEngine.AI;

public abstract class UnitBase : MonoBehaviour, IDamageable, IHealth
{
    [SerializeField] protected UnitStats stats;
    [SerializeField] protected Health health;

    [SerializeField] public LayerMask opponentLayer;
    [SerializeField] public string destinationTag;

    protected Rigidbody _rb;
    protected NavMeshAgent _agent;
    protected Collider[] _hits = new Collider[1];
    protected EntityObserver observer;

    public UnitStats Stats { get { return stats; } }

    public Health Health => health;

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
        health.OnDestroyEvent?.Invoke();
    }
}
