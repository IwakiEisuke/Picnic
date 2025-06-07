using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
{
    [SerializeField] float _duration = 1.0f;
    public float Duration => _duration;

    public void Interact()
    {
        Debug.Log("InteractableTest Interacted!");
    }
}
