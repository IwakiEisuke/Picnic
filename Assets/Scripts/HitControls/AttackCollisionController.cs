using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.AttackCollisionController)]
public class AttackCollisionController : MonoBehaviour
{
    [SerializeField] Collider[] attackColliders;

    private void Update()
    {
        for (int i = 0; i < attackColliders.Length; i++)
        {
            attackColliders[i].enabled = false;
        }
    }

    public void SetColliderActive(int i)
    {
        if (i < 0 || i >= attackColliders.Length)
        {
            Debug.LogError($"{name}| SetColliderActive�̈����͔͈͊O�ł�");
            return;
        }

        attackColliders[i].enabled = true;
    }
}
