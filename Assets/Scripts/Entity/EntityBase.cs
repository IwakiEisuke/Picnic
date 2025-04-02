using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EntityBase : MonoBehaviour, IDamageable
{
    [SerializeField] UnitStats stats;
    [SerializeField] LayerMask opponentLayer;
    [SerializeField] string destinationTag;
    [SerializeField] GameObject dead;
    [SerializeField] Health health;

    Rigidbody _rb;
    NavMeshAgent _agent;
    Collider[] _hits = new Collider[1];

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = stats.Speed;

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

    private IEnumerator MoveState()
    {
        while (true)
        {
            var targets = GameObject.FindGameObjectsWithTag(destinationTag);
            if (targets.Count() > 0)
            {
                var closest = targets.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
                _agent.stoppingDistance = stats.AttackRadius;
                _agent.SetDestination(closest.transform.position);
            }
            else
            {
                _agent.stoppingDistance = 0;
                _agent.SetDestination(Vector3.zero);
            }

            yield return null;
        }
    }

    private IEnumerator AttackState()
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
        _rb.AddForce(transform.position.normalized * other.KnockBack, ForceMode.Impulse);
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
