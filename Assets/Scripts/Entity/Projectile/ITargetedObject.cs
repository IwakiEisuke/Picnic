using UnityEngine;

/// <summary>
/// 目標地点を持つオブジェクトのインターフェース。
/// </summary>
public interface ITargetedObject
{
    public void InitializeTarget(Vector3 targetPosition, AttackData data);
}
