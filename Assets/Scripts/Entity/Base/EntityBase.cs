using System;
using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.EntityBase)]
public class EntityBase : MonoBehaviour
{
    [SerializeField] protected EntityType type;
    [SerializeField] protected EntityManager manager;
    [SerializeField] protected Vector3 pivotOffset;

    public EntityType EntityType => type;
    public EntityManager Manager => manager;
    public Vector3 CenterPosition => transform.position + transform.rotation * pivotOffset;

    public event Action OnDied;
    public event Action OnDestroyed;

    protected void InvokeOnDied() => OnDied?.Invoke();
    protected void InvokeOnDestroyed() => OnDestroyed?.Invoke();

    protected void RegisterEntityBase()
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