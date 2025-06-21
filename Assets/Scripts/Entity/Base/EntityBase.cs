using System;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField] protected EntityType type;
    public EntityType EntityType => type;
    public event Action OnDied;
    public event Action OnDestroyed;

    protected void InvokeOnDied() => OnDied?.Invoke();
    protected void InvokeOnDestroyed() => OnDestroyed?.Invoke();
}

public enum EntityType
{
    Ally,
    Enemy,
    Object
}