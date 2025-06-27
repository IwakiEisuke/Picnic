using System.Linq;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    [SerializeField] HitFilter hitFilter;

    public Transform[] ignoreTargets;

    private void OnTriggerEnter(Collider other)
    {
        if (hitFilter != null && !hitFilter.CanHit(other))
        {
            return;
        }

        Destroy(gameObject);
    }
}
