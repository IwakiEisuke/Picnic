using UnityEngine;
using UnityEngine.InputSystem;

public class UnitControlMenu : MonoBehaviour
{
    [SerializeField] UnitSelector unitSelector;
    [SerializeField] RectTransform menuPanel;
    [SerializeField] Vector3 menuOffset;
    [SerializeField] MouseInputManager mouseInput;

    void Start()
    {
        menuPanel.gameObject.SetActive(false);
        mouseInput.OpenMenu += () => ToggleMenu(Mouse.current.position.value);
        mouseInput.CloseMenu += CloseMenu;
    }

    public void ToggleMenu(Vector3 screenPos)
    {
        menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
        menuPanel.position = screenPos + menuOffset;
    }

    public void CloseMenu()
    {
        menuPanel.gameObject.SetActive(false);
    }
}
