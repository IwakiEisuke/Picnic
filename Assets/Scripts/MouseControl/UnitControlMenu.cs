using UnityEngine;
using UnityEngine.InputSystem;

public class UnitControlMenu : MonoBehaviour
{
    [SerializeField] UnitSelector unitSelector;
    [SerializeField] RectTransform menuPanel;
    [SerializeField] Vector3 menuOffset;
    [SerializeField] InputActionReference openMenuAction;

    void Start()
    {
        menuPanel.gameObject.SetActive(false);
        openMenuAction.action.canceled += _ => OpenMenu(Mouse.current.position.value);
    }

    public void OpenMenu(Vector3 screenPos)
    {
        menuPanel.gameObject.SetActive(true);
        menuPanel.position = screenPos + menuOffset;
    }

    public void CloseMenu()
    {
        menuPanel.gameObject.SetActive(false);
    }
}
