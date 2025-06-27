using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionTree", menuName = "EvolutionTree", order = 1)]
public class EvolutionTree : ScriptableObject
{
    [SerializeField] EvolutionTreeNode[] treeNodes;
    [SerializeField] EvolutionTreeNodeView viewPrefab;

    EvolutionTreeNode currentNode;
    EvolutionTreeNodeView[] views;

    UnitBase owner;

    public EvolutionTreeNode[] TreeNodes => treeNodes;
    public EvolutionTreeNode CurrentNode => currentNode;

    /// <summary>
    /// 引数のEvolutionTreeの内容をこのインスタンスにコピーします。
    /// </summary>
    /// <param name="tree"></param>
    public void Copy(EvolutionTree tree)
    {
        treeNodes = tree.treeNodes;
        viewPrefab = tree.viewPrefab;
        if (tree.currentNode != null)
        {
            currentNode = tree.currentNode;
        }
        else currentNode = treeNodes[0];
    }

    public void SetOwner(UnitBase owner)
    {
        this.owner = owner;
    }

    public GameObject GeneratePanel(Transform parent)
    {
        var parentObj = new GameObject("Tree");
        parentObj.transform.SetParent(parent, false);

        views = new EvolutionTreeNodeView[treeNodes.Length];

        for (int i = 0; i < views.Length; i++)
        {
            if (views[i] == null)
            {
                views[i] = Instantiate(viewPrefab, parentObj.transform);
            }
            views[i].Set(this, i);
        }

        return parentObj;
    }

    public void TryEvolve(int to)
    {
        if (currentNode.CanEvolve(to))
        {
            currentNode = treeNodes[to]; // 進化に成功したら現在のノードを更新
            
            if (owner == null)
            {
                Debug.LogWarning("EvolutionTree: Owner is not set. Please assign a UnitBase to the owner field.");
                return;
            }

            owner.Evolve(currentNode.SpeciePrefab);
        }
    }

    [ContextMenu(nameof(ResetInnerValue))]
    private void ResetInnerValue()
    {
        currentNode = treeNodes[0];
    }
}

[Serializable]
public class EvolutionTreeNode
{
    [SerializeField] string specieName;
    [SerializeField] string description;
    [SerializeField] EvolutionTreeEdge[] edges;
    [SerializeField] UnitBase speciePrefab;
    [SerializeField] Vector2 pos;
    [SerializeField] Sprite icon;
    [SerializeField] bool isUnlocked;

    public string SpecieName => specieName;
    public string Description => description;
    public EvolutionTreeEdge[] Edges => edges;
    public UnitBase SpeciePrefab => speciePrefab;
    public Vector2 Position => pos;
    public Sprite Icon => icon;
    public bool IsUnlocked => isUnlocked;

    public bool CanEvolve(int nodeIndex)
    {
        for (int i = 0; i < edges.Length; i++)
        {
            if (edges[i].ToIndex == nodeIndex)
            {
                if (edges[i].Cost <= GameManager.Inventory.HoneyCount)
                {
                    // 進化可能な場合、必要なコストを消費
                    GameManager.Inventory.RemoveHoney(edges[i].Cost);
                    return true;
                }

                return false; // コストが足りない場合は進化できない
            }
        }

        return false;
    }
}

[Serializable]
public class EvolutionTreeEdge
{
    [SerializeField] int toIndex;
    [SerializeField] int cost;

    public int ToIndex => toIndex; 
    public int Cost => cost;
}