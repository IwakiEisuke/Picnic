using System.Linq;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    public Transform[] ignoreTargets;
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && ignoreTargets.Contains(other.attachedRigidbody.transform)) return;
        else if (ignoreTargets.Contains(other.transform)) return;

        if (other.gameObject.layer != gameObject.layer)
        {
            Destroy(gameObject);
        }
    }
}
