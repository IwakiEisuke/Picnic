using UnityEngine;

public abstract class EffectCancelConditionBase : ScriptableObject
{
    public abstract bool IsCancel(UnitGameStatus status);
}
