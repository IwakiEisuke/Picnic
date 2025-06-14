using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public AttackData data;
}

[System.Serializable]
public class  AttackData
{
    public int damage;
    public AttackData(int damage)
    {
        this.damage = damage;
    }
}