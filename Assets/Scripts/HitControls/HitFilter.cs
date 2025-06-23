using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 衝突対象を制御するクラス
/// </summary>
public class HitFilter : MonoBehaviour
{
    [SerializeField] List<Transform> ignoreTargets;

    public bool CanHit(Transform other)
    {
        if (other.TryGetComponent<Collider>(out var collider))
        {
            if (collider.attachedRigidbody && ignoreTargets.Contains(collider.attachedRigidbody.transform))
            {
                return false;
            }
        }

        if (ignoreTargets.Contains(other.transform))
        {
            return false;
        }

        if (other.gameObject.layer != gameObject.layer)
        {
            return true;
        }

        return false;
    }

    public bool CanHit(Collider other)
    {
        if (other.attachedRigidbody && ignoreTargets.Contains(other.attachedRigidbody.transform))
        {
            return false;
        }

        if (ignoreTargets.Contains(other.transform))
        {
            return false;
        }

        if (other.gameObject.layer != gameObject.layer)
        {
            return true;
        }

        return false;
    }

    public void AddIgnore(Transform target)
    {
        if (ignoreTargets.Contains(target)) return;
        ignoreTargets.Add(target);
    }
}