using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一時的なステータス変化や状態異常を管理するクラス
/// </summary>
public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;

    readonly List<StatusEffectBase> effects = new();

    public void AddEffect(StatusEffectBase effect)
    {
        effects.Add(effect);
    }

    private void Update()
    {
        var status = new UnitGameStatus(unitBase.Stats);

        var dt = Time.deltaTime;
        foreach (var effect in effects)
        {
            if (effect.Consume(dt))
            {
                effect.Apply(status);
            }
            else
            {
                effects.Remove(effect);
            }
        }

        unitBase.status = status;
    }
}

public abstract class StatusEffectBase : ScriptableObject
{
    public float duration;
    public abstract void Apply(UnitGameStatus status);
    public bool Consume(float dt)
    {
        duration -= dt;
        return duration > 0;
    }
}