using UnityEngine;
using UnityEngine.UI;

public class EvolutionTreeView : MonoBehaviour
{
    [SerializeField] Transform treeViewParent;
    [SerializeField] Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() => PanelClose());
    }

    public void ShowTree(EvolutionTree tree)
    {
        tree.GeneratePanel(treeViewParent);
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
