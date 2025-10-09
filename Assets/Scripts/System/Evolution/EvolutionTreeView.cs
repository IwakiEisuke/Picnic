using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 進化ツリーを表示するクラス
/// </summary>
public class EvolutionTreeView : MonoBehaviour
{
    [SerializeField] Transform treeViewParent;
    [SerializeField] Button closeButton;
    [SerializeField] EvolutionTreeNodeView viewPrefab;

    GameObject currentTreePanel;

    private void Start()
    {
        closeButton.onClick.AddListener(() => PanelClose());
        PanelClose();
    }

    public void ShowTree(RuntimeEvolutionTree tree)
    {
        // 前に表示されていた進化ツリーパネルを破棄する
        if (currentTreePanel != null)
        {
            Destroy(currentTreePanel);
        }

        currentTreePanel = GeneratePanel(tree);
        PanelOpen();
    }

    GameObject GeneratePanel(RuntimeEvolutionTree tree)
    {
        var parentObj = new GameObject("Tree");
        parentObj.transform.SetParent(treeViewParent, false);

        for (int i = 0; i < tree.TreeNodes.Count; i++)
        {
            var view = Instantiate(viewPrefab, parentObj.transform);
            view.Set(tree, i);
        }

        return parentObj;
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
