using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public AttackData data;
}

[System.Serializable]
public class  AttackData
{
    public int id;
    public int damage;
    public float invincibleTime;

    public AttackData(int id, int damage, float invincibleTime)
    {
        this.id = id;
        this.damage = damage;
        this.invincibleTime = invincibleTime;
    }
}