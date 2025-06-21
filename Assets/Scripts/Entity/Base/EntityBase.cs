using System;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField] protected EntityType type;
    [SerializeField] protected EntityManager manager;
    public EntityType EntityType => type;
    public EntityManager Manager => manager;
    public event Action OnDied;
    public event Action OnDestroyed;

    protected void InvokeOnDied() => OnDied?.Invoke();
    protected void InvokeOnDestroyed() => OnDestroyed?.Invoke();

    protected void InitializeEntityBase()
    {
        manager.Subscribe(this, type);
    }
}

public enum EntityType
{
    Ally,
    Enemy,
    Object
}