using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 進化ツリーを表示するクラス
/// </summary>
public class EvolutionTreeView : MonoBehaviour
{
    [SerializeField] Transform treeViewParent;
    [SerializeField] Button closeButton;

    GameObject currentTreePanel;

    private void Start()
    {
        closeButton.onClick.AddListener(() => PanelClose());
    }

    public void ShowTree(EvolutionTree tree)
    {
        // 前に表示されていた進化ツリーパネルを破棄する
        if (currentTreePanel != null)
        {
            Destroy(currentTreePanel);
        }

        currentTreePanel = tree.GeneratePanel(treeViewParent);
    }

    public void PanelOpen()
    {
        treeViewParent.gameObject.SetActive(true);
    }

    public void PanelClose()
    {
        treeViewParent.gameObject.SetActive(false);
    }
}
