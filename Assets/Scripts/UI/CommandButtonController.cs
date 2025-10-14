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
        if (_iconImage == null)
        {
            Debug.LogWarning("CommandButtonController: Icon Image is not assigned.", this);
            return;
        }
        _iconImage.sprite = icon;
    }

    public void AddButtonAction(UnityEngine.Events.UnityAction action)
    {
        if (_button == null)
        {
            Debug.LogWarning("CommandButtonController: Button is not assigned.", this);
            return;
        }
        if (action != null)
        {
            _button.onClick.AddListener(action);
        }
    }
}
