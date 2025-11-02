using System.Collections.Generic;
using UnityEngine;

public abstract class GameInstanceRegistry<T> : ScriptableObject
{
    readonly HashSet<T> _instances = new();

    public IReadOnlyCollection<T> Instances => _instances;

    public void Register(T instance)
    {
        _instances.Add(instance);
    }

    public void Unregister(T instance)
    {
        _instances.Remove(instance);
    }
}
