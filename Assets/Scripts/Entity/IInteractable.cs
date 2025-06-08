using System;

public interface IInteractable
{
    public float Duration { get; }
    public event Action CancelInteract;
    void Interact();
}
