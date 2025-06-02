using UnityEngine;
using UnityEngine.UI;

public class UnitControlMenu : MonoBehaviour
{
    [SerializeField] UnitSelector unitSelector;
    [SerializeField] RectTransform menuPanel;
    [SerializeField] Button[] buttons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuPanel.gameObject.SetActive(false);
        unitSelector.OnSelectControlTarget += OpenMenu;

        foreach (var button in buttons)
        {
            button.onClick.AddListener(CloseMenu);
        }
    }

    void OpenMenu()
    {
        menuPanel.gameObject.SetActive(true);
        menuPanel.position = Camera.main.WorldToScreenPoint(unitSelector.ControlTarget.position);
    }

    void CloseMenu()
    {
        menuPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (unitSelector.ControlTarget == null)
        {
            menuPanel.gameObject.SetActive(false);
        }
    }
}
