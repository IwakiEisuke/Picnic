using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.GameManager)]
public class GameManager : MonoBehaviour
{
    [SerializeField] Inventory _inventory;
    [SerializeField] EntityManager _entityManager;

    public static Inventory Inventory { get; private set; }

    private void Awake()
    {
        Inventory = _inventory;
        _entityManager.Init();
    }
}
