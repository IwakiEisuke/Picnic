using UnityEngine;

public class HitManager : MonoBehaviour
{
    [SerializeField] Health health;

    public void ReceiveHit(AttackReceiveInfo info)
    {
        if (health == null)
        {
            Debug.LogWarning("Health component is not assigned in HitManager.");
            return;
        }
        health.TakeDamage(info);
    }
}

public class  AttackReceiveInfo
{
    public int damage;
}