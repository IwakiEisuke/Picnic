using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitBase : EntityBase
{
    [SerializeField] protected UnitStats stats;
    [SerializeField] protected Health health;

    [SerializeField] public LayerMask opponentLayer;
    [SerializeField] public string destinationTag;

    [SerializeField] protected AttackController attackController;
    [SerializeField] protected HitManager hitManager;
    [SerializeField] protected StatusEffectManager statusEffectManager;
    [SerializeField] protected ActionManager actionManager;

    protected Rigidbody _rb;
    protected NavMeshAgent _agent;
    protected Collider[] _hits = new Collider[1];
    protected EntityObserver observer;
    UnitGameStatus status;

    public UnitStats Stats => stats;
    public Health Health => health;
    public NavMeshAgent Agent => _agent;
    public HitManager HitManager => hitManager;
    public StatusEffectManager StatusEffectManager => statusEffectManager;
    public ActionManager ActionManager => actionManager;
    public AttackController AttackController => attackController;
    public UnitGameStatus Status => status;

    protected virtual void Awake()
    {
        RegisterEntityBase();
        InitializeUnitBase();
    }

    protected virtual void Update()
    {
        _agent.speed = status.speed;
    }

    protected void InitializeUnitBase()
    {
        status = new UnitGameStatus(stats, this);

        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = status.speed;
        health.OnDied.AddListener(Die);
        observer = new EntityObserver(stats.name);
        observer.Register();
        actionManager.SetActions(stats.Actions);
    }

    public void Die()
    {
        observer.Remove();
        InvokeOnDied();
        StopAllCoroutines();
    }

    private void OnDrawGizmos()
    {
        if (status != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, status.attackRadius);
        }
    }

    private void OnDestroy()
    {
        InvokeOnDestroyed();
    }
}