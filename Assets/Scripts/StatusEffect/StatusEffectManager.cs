using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一時的なステータス変化や状態異常を管理するクラス
/// </summary>
public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;
    [SerializeField] StatusEffectAssetBase effectTest;

    readonly List<StatusEffector> effects = new();

    public void AddEffect(StatusEffectAssetBase effect)
    {
        effects.Add(new(effect));
    }

    private void Update()
    {
        var status = new UnitGameStatus(unitBase.Stats);

        var consumed = new List<StatusEffector>();
        var dt = Time.deltaTime;
        foreach (var effect in effects)
        {
            if (effect.Consume(dt))
            {
                effect.Apply(status);
            }
            else
            {
                consumed.Add(effect);
            }
        }

        foreach (var cons in consumed)
        {
            effects.Remove(cons);
        }

        unitBase.status = status;
    }

    private void Start()
    {
        if (effectTest != null)
        {
            AddEffect(effectTest);
        }
    }
}

/// <summary>
/// 実行時ステータスエフェクト
/// </summary>
public class StatusEffector
{
    readonly StatusEffectAssetBase effect;
    float duration;

    public StatusEffector(StatusEffectAssetBase effect)
    {
        this.effect = effect;
        duration = effect.Duration;
    }

    public bool Consume(float dt)
    {
        duration -= dt;
        if (duration <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Apply(UnitGameStatus status)
    {
        effect.Apply(status);
    }
}

/// <summary>
/// ステータスエフェクトのテンプレート
/// </summary>
public abstract class StatusEffectAssetBase : ScriptableObject
{
    protected float duration;
    public float Duration;
    public abstract void Apply(UnitGameStatus status);
}