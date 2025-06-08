using UnityEngine;

public class Flower : MonoBehaviour, IInteractable
{
    [SerializeField] float duration;
    [SerializeField] int remainNectar;
    [SerializeField] int amountNectarOnce;
    public float Duration => duration;

    public void Interact()
    {
        if (remainNectar > 0)
        {
            remainNectar -= amountNectarOnce;
            GameManager.Inventory.AddHoney(amountNectarOnce);
        }
    }
}
