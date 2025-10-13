using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コマンドメニューのコマンド実行ボタンを管理するクラス
/// </summary>
public class CommandButtonController : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] Button _button;

    public void SetIcon(Sprite icon)
    {
        _iconImage.sprite = icon;
    }

    public void AddButtonAction(UnityEngine.Events.UnityAction action)
    {
        _button.onClick.AddListener(action);
    }
}
