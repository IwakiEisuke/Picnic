using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityManager", menuName = "EntityManager")]
public class EntityManager : ScriptableObject
{
    readonly Dictionary<EntityType, List<EntityBase>> entities = new();

    public void Init()
    {
        entities.Clear();
        entities.Add(EntityType.Ally, new());
        entities.Add(EntityType.Enemy, new());
        entities.Add(EntityType.Object, new());
    }

    private EntityType GetEntityType(EntityBase entity)
    {
        return entity.EntityType;
    }

    private EntityType GetOpponentType(EntityType type)
    {
        if (type == EntityType.Ally)
        {
            return EntityType.Enemy;
        }
        else if (type == EntityType.Enemy)
        {
            return EntityType.Ally;
        }

        return EntityType.Object;
    }

    public void Subscribe(EntityBase entity, EntityType type)
    {
        entities[type].Add(entity);
        entity.OnDestroyed += () => UnSubscribe(entity);
    }

    public void UnSubscribe(EntityBase entity)
    {
        var type = GetEntityType(entity);
        entities[type].Remove(entity);
    }

    /// <summary>
    /// 周囲のエンティティを取得
    /// </summary>
    public IEnumerable<EntityBase> GetEntityAround(Vector3 position, float radius, EntityType type)
    {
        var targets = entities[type];
        return targets.Where(x => (x.transform.position - position).sqrMagnitude < radius * radius);
    }

    /// <summary>
    /// 周囲の敵勢力エンティティを取得
    /// </summary>
    public IEnumerable<EntityBase> GetOpponentAround(Vector3 position, float radius, EntityType type)
    {
        return GetEntityAround(position, radius, GetOpponentType(type));
    }
}
