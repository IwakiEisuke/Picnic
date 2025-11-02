using System.Collections.Generic;
using UnityEngine;

public abstract class GameInstanceRegistry<T> : ScriptableObject
{
    readonly HashSet<T> _instances = new();

    public IReadOnlyCollection<T> Instances => _instances;

    public void Register(T instance)
    {
        if (instance == null)
        {
            Debug.LogWarning($"{GetType()}: Attempted to register a null instance");
            return;
        }

        if (!_instances.Add(instance))
        {
            Debug.LogWarning($"{GetType()}: Instance {typeof(T).Name} is already registered");
        }
    }

    public void Unregister(T instance)
    {
        if (instance == null)
        {
            Debug.LogWarning($"{GetType()}: Attempted to unregister a null instance from registry");
            return;
        }
        _instances.Remove(instance);
    }
}
