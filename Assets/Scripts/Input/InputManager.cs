using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] MouseInputManager mouseInputManager;

    public MouseInputManager MouseInput => mouseInputManager;

    private void Start()
    {
        mouseInputManager.Init();
    }

    private void Update()
    {
        mouseInputManager.Update();
    }

    private void OnDestroy()
    {
        mouseInputManager.ResetActions();
    }
}
