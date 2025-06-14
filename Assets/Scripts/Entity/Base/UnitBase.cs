using System;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder((int)ExecutionOrder.UnitBase)]
public abstract class UnitBase : MonoBehaviour, IUnit
{
    [SerializeField] protected UnitStats stats;
    [SerializeField] protected Health health;

    [SerializeField] public LayerMask opponentLayer;
    [SerializeField] public string destinationTag;

    [SerializeField] protected EvolutionTree evolutionTreeAsset;
    [SerializeField] protected Transform evolutionTreeViewParent;

    [SerializeField] protected AttackCollisionController collisionController;

    protected Rigidbody _rb;
    protected NavMeshAgent _agent;
    protected Collider[] _hits = new Collider[1];
    protected EntityObserver observer;

    public UnitStats Stats { get { return stats; } }

    public Health Health => health;
    public NavMeshAgent Agent => _agent;

    [SerializeField] protected EvolutionTree evolutionTree;
    public EvolutionTree EvolutionTree => evolutionTree;
    public AttackCollisionController CollisionController => collisionController;

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
        health.OnDied.AddListener(Die);
        observer = new EntityObserver(stats.name);
        observer.Register();

        if (evolutionTree == null && evolutionTreeAsset != null)
        {
            evolutionTree = ScriptableObject.CreateInstance<EvolutionTree>();
            evolutionTree.owner = this;
            evolutionTree.Copy(evolutionTreeAsset);
            evolutionTree.GeneratePanel(evolutionTreeViewParent);
            PanelClose();
        }
    }

    public void Die()
    {
        observer.Remove();
        StopAllCoroutines();
    }

    public void PanelOpen()
    {
        evolutionTreeViewParent.gameObject.SetActive(true);
    }

    public void PanelClose()
    {
        evolutionTreeViewParent.gameObject.SetActive(false);
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