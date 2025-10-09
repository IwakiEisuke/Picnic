using UnityEngine;

public class Test_EvolutionTreeViewTester : MonoBehaviour
{
    [SerializeField] EvolutionTree tree;
    [SerializeField] EvolutionTreeView view;

    private void Start()
    {
        SetTree();
    }

    [ContextMenu("SetTree")]
    public void SetTree()
    {
        var testTree = new RuntimeEvolutionTree();
        testTree.SetTree(tree);
        view.ShowTree(testTree);
    }
}
