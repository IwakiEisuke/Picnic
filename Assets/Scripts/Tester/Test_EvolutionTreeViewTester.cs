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
        var testTree = ScriptableObject.CreateInstance<EvolutionTree>();
        testTree.Copy(tree);
        view.ShowTree(testTree);
    }
}
