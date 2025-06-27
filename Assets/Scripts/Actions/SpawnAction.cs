using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnAction", menuName = "Actions/SpawnAction")]
public class SpawnAction : ActionBase
{
    [SerializeField] int maxSpawnCount = 3;
    [SerializeField] float spawnRadius = 1.5f;
    [SerializeField] bool canMoveWhileExecuting;
    [SerializeField] Vector3 offset;

    [SerializeField] GameObject spawnPrefab;

    readonly float[] rangeMultipliers = { 1, 1.2f, 1.5f };

    readonly List<GameObject> spawnObjects = new();

    float TriggerRange => rangeMultipliers[level] * _status.attackRadius;

    public override float Evaluate()
    {
        for (int i = spawnObjects.Count - 1; i >= 0; i--)
        {
            if (spawnObjects[i] == null || !spawnObjects[i].activeInHierarchy)
            {
                spawnObjects.RemoveAt(i);
            }
        }

        if (spawnObjects.Count >= maxSpawnCount) return -1f;

        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, TriggerRange, _parent.EntityType, opponent, selfInclude, out var target))
        {
            return 1;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        if (!canMoveWhileExecuting) _agent.SetDestination(transform.position);

        if (spawnObjects.Count >= maxSpawnCount) return new ActionExecuteInfo(false, this);
        {

            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius + this.offset;
            var obj = Instantiate(spawnPrefab, transform.position + offset, Quaternion.identity);
            obj.layer = _parent.Manager.GetEntityLayer(_parent, !opponent);

            if (obj.TryGetComponent<AttackCollider>(out var attackCollider))
            {
                if (!opponent) attackCollider.hitFilter.AddIgnore(transform);
            }

            spawnObjects.Add(obj);
        }

        return new ActionExecuteInfo(true, this);
    }
}
