using System;
using UnityEngine;

public class Flower : EntityBase, IInteractable
{
    [SerializeField] float duration;
    [SerializeField] int remainNectar;
    [SerializeField] int amountNectarOnce;
    public float Duration => duration;
    public bool IsInteractable => remainNectar > 0;

    private void Awake()
    {
        RegisterEntityBase();
    }

    public void Interact()
    {
        if (remainNectar > 0)
        {
            remainNectar -= amountNectarOnce;
            GameManager.Inventory.AddHoney(amountNectarOnce);
        }
    }
}
