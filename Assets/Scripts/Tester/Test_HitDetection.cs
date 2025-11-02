using UnityEngine;

public class Test_HitDetection : MonoBehaviour
{
    [SerializeField] HitManager hitManager;

    void Update()
    {
        if (hitManager.IsAttacked)
        {
            Debug.Log($"{name} has been attacked!");
        }
        if (hitManager.IsDamaged)
        {
            Debug.Log($"{name} has taken damage!");
        }
    }
}
