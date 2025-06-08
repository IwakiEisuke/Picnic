using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.GameManager)]
public class GameManager : MonoBehaviour
{
    [SerializeField] Inventory _inventory;

    public static Inventory Inventory { get; private set; }

    private void Awake()
    {
        Inventory = _inventory;
    }
}
