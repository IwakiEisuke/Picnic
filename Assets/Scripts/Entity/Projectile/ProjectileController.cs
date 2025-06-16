using UnityEngine;

/// <summary>
/// 弾オブジェクトを飛ばす／消滅させる
/// </summary>
public class ProjectileController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public bool destroyOnHit = true;
    public float lifeRange = 5f;

    private void Start()
    {
        rb.linearVelocity = transform.forward * 10f;
        Destroy(gameObject, lifeRange / rb.linearVelocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (destroyOnHit)
        {
            if (other.gameObject.layer != gameObject.layer)
            {
                Destroy(gameObject);
            }
        }
    }
}
