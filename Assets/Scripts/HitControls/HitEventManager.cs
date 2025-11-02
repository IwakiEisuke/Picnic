using System.Linq;
using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.HitEventManager)]
public class HitEventManager : MonoBehaviour
{
    [SerializeField] HitManagerRegistry _hitManagerRegistry;

    private void Awake()
    {
        if (_hitManagerRegistry == null)
        {
            Debug.LogWarning("HitEventManager: HitManagerRegistryがアサインされていません");
        }
    }

    void Update()
    {
        if (_hitManagerRegistry == null) return;

        foreach (var hitManager in _hitManagerRegistry.Instances.ToList())
        {
            hitManager.ClearHitStates();
        }
    }
}
