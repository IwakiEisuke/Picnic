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

    /// <summary>
    /// エンティティのレイヤーを取得
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="opponent">エンティティの敵勢力レイヤーを取得するか</param>
    /// <returns></returns>
    public int GetEntityLayer(EntityBase entity, bool opponent)
    {
        var type = GetEntityType(entity);
        if (opponent) 
            type = GetOpponentType(type);

        return type switch
        {
            EntityType.Ally => LayerMask.NameToLayer("Ally"),
            EntityType.Enemy => LayerMask.NameToLayer("Enemy"),
            EntityType.Object => LayerMask.NameToLayer("Object"),
            _ => LayerMask.NameToLayer("Default"),
        };
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
    public IEnumerable<EntityBase> GetEntityAround(EntityBase user, Vector3 position, float radius, EntityType type, bool opponent, bool selfInclude)
    {
        DebugUtility.DrawSphere(position, radius, Color.green);

        if (opponent)
        {
            type = GetOpponentType(type);
        }

        var targets = entities[type];
        var result = targets.Where(x => (x.CenterPosition - position).sqrMagnitude < radius * radius);

        if (selfInclude)
        {
            return result;
        }
        else
        {
            return result.Where(x => x != user);
        }
    }

    /// <summary>
    /// 周囲のエンティティから最も近いものを取得
    /// </summary>
    public bool TryGetNearestEntityAround(EntityBase user, Vector3 position, float radius, EntityType type, bool opponent, bool selfInclude, out EntityBase target)
    {
        DebugUtility.DrawSphere(position, radius, Color.green);

        if (opponent)
        {
            type = GetOpponentType(type);
        }

        var targets = entities[type].AsEnumerable();

        if (!selfInclude)
        {
            targets = targets.Where(x => x != user);
        }

        var entity = targets.OrderBy(x => (x.CenterPosition - position).sqrMagnitude).FirstOrDefault();

        if (entity != null && (entity.CenterPosition - position).sqrMagnitude < radius * radius)
        {
            target = entity;
            return true;
        }

        target = null;
        return false;
    }
}
