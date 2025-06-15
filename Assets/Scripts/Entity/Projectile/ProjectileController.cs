using UnityEngine;

/// <summary>
/// 弾オブジェクトを飛ばす／消滅させる
/// </summary>
public class ProjectileController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private void Start()
    {
        rb.linearVelocity = transform.forward * 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
