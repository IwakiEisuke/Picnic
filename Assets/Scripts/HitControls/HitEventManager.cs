using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.HitEventManager)]
public class HitEventManager : MonoBehaviour
{
    HitManager[] _hitManagers;

    void Start()
    {
        _hitManagers = FindObjectsByType<HitManager>(FindObjectsSortMode.None);
    }

    void Update()
    {
        foreach (var hitManager in _hitManagers)
        {
            hitManager.ClearHitStates();
        }
    }
}
