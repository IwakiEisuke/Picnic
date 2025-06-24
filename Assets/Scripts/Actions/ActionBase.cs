using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class ActionBase : ScriptableObject
{
    [SerializeField, Range(1, 3)] protected int level = 1;
    [Tooltip("次のアクションを選択できるようになるまでの時間")] 
    [SerializeField] protected float delayTime = 1;
    [Tooltip("このアクションが再発動可能になるまでの時間")] 
    [SerializeField] protected float cooldownTime = 1;
    [Tooltip("アクションの持続(Update)時間")] 
    [SerializeField] protected float duration;
    [Tooltip("アクションのループ回数")] 
    [SerializeField] protected int loopCount;
    [Tooltip("ループ間の待機時間")] 
    [SerializeField] protected float loopInterval;
    [SerializeField] protected bool opponent = true;
    [SerializeField] protected bool selfInclude = false;

    public float DelayTime => delayTime;
    public float CooldownTime => cooldownTime;
    public float Duration => duration;
    public int LoopCount => loopCount;
    public float LoopInterval => loopInterval;

    protected UnitBase _parent;
    protected NavMeshAgent _agent;
    protected UnitGameStatus _status;
    protected AttackController _attackController;
    protected Transform transform;

    readonly Collider[] _hits = new Collider[10]; // OverlapSphereの結果を格納する配列

    public void Initialize(UnitBase parent)
    {
        _parent = parent;
        _agent = parent.Agent;
        _status = parent.Status;
        _attackController = parent.GetComponent<AttackController>();
        transform = parent.transform;
        OnInitialize();
    }

    /// <summary>
    /// 派生クラスで初期化処理を行うためのメソッド
    /// </summary>
    protected virtual void OnInitialize() { }

    public abstract float Evaluate();
    public abstract ActionExecuteInfo Execute();
    public virtual void Update() { }

    protected Transform[] LaserCast(Vector3 origin, Vector3 direction, float distance, float boxSize, LayerMask layerMask)
    {
        var center = origin + direction * (distance / 2);
        var halfExtents = new Vector3(boxSize, boxSize, distance / 2);
        var rot = Quaternion.LookRotation(direction);
        var hitCount = Physics.OverlapBoxNonAlloc(center, halfExtents, _hits, rot, layerMask.value);
        DebugUtility.DrawWireBoxOriented(center, halfExtents, rot, Color.cyan);
        return GetRootTransformsOrder(_hits, center, hitCount);
    }

    Transform[] GetRootTransforms(Collider[] components, Vector3 position, int hitCount)
    {
        return components.Take(hitCount)
            // 主要コンポーネントのアタッチされているTransformを取得するためRigidbodyが存在する場合はRigidbodyのTransformを、そうでない場合はColliderのTransformを使用
            .Select(c => c.attachedRigidbody != null ? c.attachedRigidbody.transform : c.transform).ToArray();
    }

    Transform[] GetRootTransformsOrder(Collider[] components, Vector3 position, int hitCount)
    {
        return components.Take(hitCount)
            // 主要コンポーネントのアタッチされているTransformを取得するためRigidbodyが存在する場合はRigidbodyのTransformを、そうでない場合はColliderのTransformを使用
            .Select(c => c.attachedRigidbody != null ? c.attachedRigidbody.transform : c.transform)
            // 近い順にソート
            .OrderBy(c => (c.transform.position - position).sqrMagnitude).ToArray();
    }
}

/// <summary>
/// アクションの実行結果を表す構造体
/// </summary>
public readonly struct ActionExecuteInfo
{
    public readonly bool success;
    public readonly ActionBase action;
    public readonly float delay;
    public readonly float cooldownTime;
    public readonly float duration;
    public readonly int loopCount;
    public readonly float loopInterval;

    public ActionExecuteInfo(bool success, ActionBase action)
    {
        this.success = success;
        this.action = action;
        this.delay = action.DelayTime;
        this.cooldownTime = action.CooldownTime;
        this.duration = action.Duration;
        this.loopCount = action.LoopCount;
        this.loopInterval = action.LoopInterval;
    }

    public ActionExecuteInfo(bool success, ActionBase action, float delayTime, float cooldownTime, float duration, int loop, float loopInterval)
    {
        this.success = success;
        this.action = action;
        this.delay = delayTime;
        this.cooldownTime = cooldownTime;
        this.duration = duration;
        this.loopCount = loop;
        this.loopInterval = loopInterval;
    }

    public override readonly string ToString() => $"{action.name} (Interval: {cooldownTime})";
}