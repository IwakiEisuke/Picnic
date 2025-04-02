using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected UnitStats stats;
    [SerializeField] protected LayerMask opponentLayer;
    [SerializeField] protected string destinationTag;
    [SerializeField] protected GameObject dead;
    [SerializeField] protected Health health;

    protected Rigidbody _rb;
    protected NavMeshAgent _agent;
    protected Collider[] _hits = new Collider[1];

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();

        health.OnDied += () =>
        {
            print($"{name}: Dead");

            Destroy(gameObject);

            StopAllCoroutines();

            if (dead)
            {
                Instantiate(dead, transform.position, transform.rotation);
            }
        };
    }

    private void Start()
    {
        StartCoroutine(MoveState());
        StartCoroutine(AttackState());
    }

    private void Update()
    {
        _agent.speed = stats.Speed;
    }

    virtual protected IEnumerator MoveState()
    {
        while (true)
        {
            var random = Random.insideUnitCircle;
            var dir = new Vector3(random.x, 0, random.y);
            _agent.SetDestination(transform.position + dir * stats.Speed);

            yield return new WaitForSeconds(1);
        }
    }

    virtual protected IEnumerator AttackState()
    {
        while (true)
        {
            if (CheckAround())
            {
                Attack();
                yield return new WaitForSeconds(stats.AttackInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void Attack()
    {
        print($"{name}: Attack");
        foreach (var damageable in _hits[0].GetComponentsInParent<IDamageable>())
        {
            damageable.TakeDamage(stats);
        }
    }

    private bool CheckAround()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, stats.AttackRadius, _hits, opponentLayer.value) > 0;
    }

    public void TakeDamage(UnitStats other)
    {
        _agent.velocity = transform.position.normalized * other.KnockBack;
    }

    private void OnDrawGizmos()
    {
        if (stats)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, stats.AttackRadius);
        }
    }
}
