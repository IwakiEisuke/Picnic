using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユニット毎の進化ツリーの管理と表示を行うクラス
/// </summary>
public class EvolutionTreeController : MonoBehaviour
{
    [SerializeField] UnitBase owner;
    [SerializeField] Transform treeViewParent;
    [SerializeField] EvolutionTree evolutionTreeAsset;
    [SerializeField] Button closeButton;

    EvolutionTree evolutionTree;

    private void Start()
    {
        if (evolutionTree == null && evolutionTreeAsset != null)
        {
            evolutionTree = ScriptableObject.CreateInstance<EvolutionTree>();
            evolutionTree.Copy(evolutionTreeAsset);
            evolutionTree.GeneratePanel(treeViewParent);
            if (owner)
            {
                evolutionTree.SetOwner(owner);
                PanelClose();
            }
            else
            {
                Debug.LogWarning("EvolutionTreeView: Owner is not set. Please assign a UnitBase to the owner field.");
            }
        }

        closeButton.onClick.AddListener(() => PanelClose());
    }

    public void PanelOpen()
    {
        treeViewParent.gameObject.SetActive(true);
    }

    public void PanelClose()
    {
        treeViewParent.gameObject.SetActive(false);
    }

    public void Copy(EvolutionTreeController origin)
    {
        evolutionTree = origin.evolutionTree;
        evolutionTree.GeneratePanel(treeViewParent);
    }
}
