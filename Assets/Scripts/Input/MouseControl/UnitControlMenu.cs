using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnitControlMenu : MonoBehaviour
{
    [SerializeField] UnitSelector unitSelector;
    [SerializeField] RectTransform menuPanel;
    [SerializeField] Vector3 menuOffset;
    [SerializeField] MouseInputManager mouseInput;
    [SerializeField] GameObject canvasScaler;

    public bool IsMenuOpened => menuPanel.gameObject.activeSelf;

    void Start()
    {
        menuPanel.gameObject.SetActive(false);
        mouseInput.OpenMenu += () => ToggleMenu(Mouse.current.position.value);
        mouseInput.CloseMenu += CloseMenu;
    }

    public void ToggleMenu(Vector3 screenPos)
    {
        menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
        
        var scaler = canvasScaler.GetComponent<CanvasScaler>();
        var scaleX = Screen.width / scaler.referenceResolution.x;
        var scaleY = Screen.height / scaler.referenceResolution.y;

        var scaledOffset = new Vector3(menuOffset.x * scaleX, menuOffset.y * scaleY);

        menuPanel.position = screenPos + scaledOffset;
        Debug.Log(screenPos);
    }

    public void CloseMenu()
    {
        menuPanel.gameObject.SetActive(false);
    }
}
