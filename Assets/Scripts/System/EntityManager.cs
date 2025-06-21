using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] List<EntityBase> allies;
    [SerializeField] List<EntityBase> enemies;
    [SerializeField] List<EntityBase> objects;

    readonly Dictionary<EntityType, List<EntityBase>> entities = new();

    private void Start()
    {
        entities.Add(EntityType.Ally, allies);
        entities.Add(EntityType.Enemy, enemies);
        entities.Add(EntityType.Object, objects);
    }

    public EntityType GetEntityType(EntityBase entity)
    {
        return entity.EntityType;
    }

    public void Add(EntityBase entity)
    {
        var type = GetEntityType(entity);
        entities[type].Add(entity);
        entity.OnDestroyed += () => Remove(entity);
    }

    public void Remove(EntityBase entity)
    {
        var type = GetEntityType(entity);
        entities[type].Remove(entity);
    }

    public void GetEntityAround(Vector3 position, float radius, EntityType type)
    {

    }

    public void GetOpponentAround(Vector3 position, float radius, EntityType type)
    {

    }
}