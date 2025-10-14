using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ユニットの操作メニューを管理するクラス
/// </summary>
public class UnitControlMenu : MonoBehaviour
{
    [SerializeField] RectTransform menuPanel;
    [SerializeField] Vector3 menuOffset;
    [SerializeField] MouseInputManager mouseInput;
    [SerializeField] GameObject canvasScaler;
    [SerializeField] CommandMenuGenerator commandMenuGenerator;

    public bool IsMenuOpened => menuPanel.gameObject.activeSelf;

    void Start()
    {
        menuPanel.gameObject.SetActive(false);
        mouseInput.OpenMenu += () => ToggleMenu(Mouse.current.position.value);
        mouseInput.CloseMenu += CloseMenu;
    }

    public void ToggleMenu(Vector3 screenPos)
    {
        if (!menuPanel.gameObject.activeSelf)
        {
            OpenMenu(screenPos);
        }
        else
        {
            CloseMenu();
        }
    }

    public void OpenMenu(Vector3 screenPos)
    {
        menuPanel.gameObject.SetActive(true);

        // メニューの表示位置を計算
        var scaler = canvasScaler.GetComponent<CanvasScaler>();
        var scaleX = Screen.width / scaler.referenceResolution.x;
        var scaleY = Screen.height / scaler.referenceResolution.y;
        var scaledOffset = new Vector3(menuOffset.x * scaleX, menuOffset.y * scaleY);

        menuPanel.position = screenPos + scaledOffset;

        // コマンドメニューを生成  
        commandMenuGenerator.CreateMenu();
    }

    public void CloseMenu()
    {
        // コマンドメニューをクリア
        commandMenuGenerator.ClearMenu();
        // メニューを非表示
        menuPanel.gameObject.SetActive(false);
    }
}
