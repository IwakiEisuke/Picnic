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

    NavMeshAgent agent;
    Collider[] hit = new Collider[1];

    int _currentHealth;

    Coroutine _currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.Speed;

        _currentHealth = stats.MaxHealth;
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
                agent.stoppingDistance = stats.AttackRadius;
                agent.SetDestination(closest.transform.position);
            }
            else
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(Vector3.zero);
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
        hit[0].GetComponentInParent<IDamageable>().TakeDamage(stats.Atk);
    }

    private bool CheckAround()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, stats.AttackRadius, hit, opponentLayer.value) > 0;
    }

    public void TakeDamage(int value)
    {
        _currentHealth -= value;

        if (_currentHealth <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        print($"{name}: Dead");

        Destroy(gameObject);

        StopAllCoroutines();

        if (dead)
        {
            Instantiate(dead, transform.position, transform.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stats.AttackRadius);
    }
}
