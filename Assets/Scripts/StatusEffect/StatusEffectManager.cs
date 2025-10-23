using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 一時的なステータス変化や状態異常を管理するクラス
/// </summary>
public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;
    [SerializeField] StatusEffectAssetBase[] initialStatusEffects;

    readonly List<StatusEffector> effects = new();
    readonly List<StatusEffector> _consumed = new();

    public void AddEffect(StatusEffectAssetBase effect)
    {
        if (effect == null)
        {
            Debug.LogWarning($"{name}| StatusEffectManagerのAddEffectにNullが渡されました");
            return;
        }

        var effector = new StatusEffector(effect);
        effects.Add(effector);
    }

    public void AddEffect(StatusEffectAssetBase[] effects)
    {
        foreach (var effect in effects)
        {
            AddEffect(effect);
        }
    }

    public void RemoveEffect(StatusEffector effect)
    {
        effects.Remove(effect);
    }

    public void ClearEffects()
    {
        effects.Clear();
    }

    public void ClearEffects(EffectType type, int clearCount)
    {
        _consumed.AddRange(effects.Where(e => e.EffectType == type && clearCount-- > 0));
    }

    public List<StatusEffector> GetEffects(EffectType type)
    {
        return effects.FindAll(e => e.EffectType == type);
    }

    private void Update()
    {
        var status = new UnitGameStatus(unitBase.Stats, unitBase);

        var dt = Time.deltaTime;
        foreach (var effect in effects)
        {
            effect.Apply(status);

            if (!effect.Consume(dt) || effect.IsCanceled(status))
            {
                _consumed.Add(effect);
            }
        }

        foreach (var cons in _consumed)
        {
            effects.Remove(cons);
        }
        _consumed.Clear();

        unitBase.Status.Replace(status);
    }

    private void Start()
    {
        AddEffect(initialStatusEffects);
    }
}

/// <summary>
/// 実行時ステータスエフェクト
/// </summary>
public class StatusEffector
{
    readonly StatusEffectAssetBase effect;
    float duration;

    public EffectType EffectType => effect.EffectType;

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

    public bool IsCanceled(UnitGameStatus status)
    {
        return effect.IsCanceled(status);
    }
}

/// <summary>
/// ステータスエフェクトのテンプレート
/// </summary>
public abstract class StatusEffectAssetBase : ScriptableObject
{
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected float _duration;
    [SerializeField] protected EffectType _effectType;
    [SerializeField] protected EffectCancelConditionBase[] _cancelConditions;

    public Sprite Icon => _icon;
    public float Duration => _duration;
    public EffectType EffectType => _effectType;

    public abstract void Apply(UnitGameStatus status);

    /// <summary>
    /// このステータスエフェクトがどれくらい有効かを評価するメソッド。
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public abstract float Evaluate(UnitBase unit);

    public bool IsCanceled(UnitGameStatus status)
    {
        foreach (var condition in _cancelConditions)
        {
            if (condition.IsCancel(status))
            {
                return true;
            }
        }
        return false;
    }
}

public enum EffectType
{
    Buff,
    Debuff,
    Neutral
}