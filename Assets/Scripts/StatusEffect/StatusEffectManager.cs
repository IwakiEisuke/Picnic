﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一時的なステータス変化や状態異常を管理するクラス
/// </summary>
public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;
    [SerializeField] StatusEffectAssetBase[] initialStatusEffects;

    readonly List<StatusEffector> effects = new();

    public void AddEffect(StatusEffectAssetBase effect)
    {
        if (effect == null)
        {
            Debug.LogWarning($"{name}| StatusEffectManagerのAddEffectにNullが渡されました");
            return;
        }

        var effector = new StatusEffector(effect);
        effector.SetCancelCondition(unitBase.Status, () => RemoveEffect(effector));
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

    private void Update()
    {
        var status = new UnitGameStatus(unitBase.Stats, unitBase);

        var consumed = new List<StatusEffector>();
        var dt = Time.deltaTime;
        foreach (var effect in effects)
        {
            effect.Apply(status);
            
            if (!effect.Consume(dt))
            {
                consumed.Add(effect);
            }
        }

        foreach (var cons in consumed)
        {
            effects.Remove(cons);
        }

        unitBase.Status.Replace(status);
    }

    private void Start()
    {
        for (int i = 0; i < initialStatusEffects.Length; i++)
        {
            AddEffect(initialStatusEffects[i]);
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
    event Action CancelAction;

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

    public void Cancel()
    {
        CancelAction?.Invoke();
    }

    public void SetCancelCondition(UnitGameStatus status, Action cancelAction)
    {
        CancelAction += cancelAction;
        effect.SetCancelCondition(status, this);
    }
}

/// <summary>
/// ステータスエフェクトのテンプレート
/// </summary>
public abstract class StatusEffectAssetBase : ScriptableObject
{
    [SerializeField] protected float _duration;

    public float Duration => _duration;

    public abstract void Apply(UnitGameStatus status);

    /// <summary>
    /// コールバックからキャンセルできるようにしたいときに使用する。
    /// </summary>
    public virtual void SetCancelCondition(UnitGameStatus status, StatusEffector effector)
    {
        
    }

    /// <summary>
    /// このステータスエフェクトがどれくらい有効かを評価するメソッド。
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public abstract float Evaluate(UnitBase unit);
}