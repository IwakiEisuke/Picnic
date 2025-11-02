using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.HitEventManager)]
public class HitEventManager : MonoBehaviour
{
    [SerializeField] HitManagerRegistry _hitManagerRegistry;

    void Update()
    {
        foreach (var hitManager in _hitManagerRegistry.Instances)
        {
            hitManager.ClearHitStates();
        }
    }
}
