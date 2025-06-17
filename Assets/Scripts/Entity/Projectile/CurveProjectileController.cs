using UnityEngine;

/// <summary>
/// 放射状投射
/// </summary>
public class CurveProjectileController : MonoBehaviour, ITargetedObject, IAreaObject
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float time = 10f;
    [SerializeField] float curveHeight = 1f;
    [SerializeField] float gravity;
    [SerializeField] GameObject onEndSpawnPrefab;
    [SerializeField] AttackData data;
    
    public Vector3 targetPos;

    float t;
    float onEndSpawnObjRadius;

    private void Start()
    {
        // 高さ考慮してないけどOK
        var v0 = curveHeight / time;
        gravity = 2 * v0 * v0 / curveHeight;

        rb.linearVelocity = new Vector3(
            (targetPos.x - transform.position.x) / time,
            v0,
            (targetPos.z - transform.position.z) / time
        );
    }

    private void Update()
    {
        rb.linearVelocity += -gravity * Time.deltaTime * Vector3.up;
        t += Time.deltaTime;
        if (t >= time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != gameObject.layer)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        var obj = Instantiate(onEndSpawnPrefab, transform.position, Quaternion.identity);
        obj.layer = gameObject.layer;
        if (obj.TryGetComponent<AttackCollider>(out var collider))
        {
            collider.data = data;
        }

        if (obj.TryGetComponent<IAreaObject>(out var area))
        {
            area.InitializeArea(onEndSpawnObjRadius);
        }
    }

    public void InitializeTarget(Vector3 targetPosition, AttackData data)
    {
        targetPos = targetPosition;
        this.data = data;
    }

    /// <summary>
    /// 衝突時に生成するオブジェクトがIAreaObjectを実装していた場合にRadiusを渡す用
    /// </summary>
    public void InitializeArea(float radius)
    {
        onEndSpawnObjRadius = radius;
    }
}
