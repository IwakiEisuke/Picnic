using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public AttackData data;
    public HitFilter hitFilter;
}

[System.Serializable]
public class  AttackData
{
    public int id;
    public int damage;
    public float invincibleTime;
    public StatusEffectAssetBase[] statusEffects;

    public AttackData(int id, int damage, float invincibleTime, StatusEffectAssetBase[] statusEffects)
    {
        this.id = id;
        this.damage = damage;
        this.invincibleTime = invincibleTime;
        this.statusEffects = statusEffects;
    }
}