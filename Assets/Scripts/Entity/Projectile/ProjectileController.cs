using System.Linq;
using UnityEngine;

/// <summary>
/// 弾オブジェクトを飛ばす／消滅させる
/// </summary>
public class ProjectileController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public float speed = 10f;
    public float lifeRange = 5f;

    private void Start()
    {
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeRange / rb.linearVelocity.magnitude);
    }
}
