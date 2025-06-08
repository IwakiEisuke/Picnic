using System;
using UnityEngine;

public class EvolutionTree : MonoBehaviour
{
    [SerializeField] EvolutionTreeNode[] treeNodes;
    [SerializeField] EvolutionTreeNodeView viewPrefab;
    [SerializeField] EvolutionTreeNodeView[] views;

    EvolutionTreeNode currentNode;

    bool initialized;

    public EvolutionTreeNode[] TreeNodes => treeNodes;

    private void Start()
    {
        Close();
        if (initialized) return;
        GeneratePanel();
        currentNode = treeNodes[0]; // 初期ノードを設定
    }

    public void Copy(EvolutionTree tree)
    {
        treeNodes = tree.treeNodes;
        currentNode = tree.currentNode;
        GeneratePanel();
        initialized = true;
    }

    public void GeneratePanel()
    {
        views = new EvolutionTreeNodeView[treeNodes.Length];

        for (int i = 0; i < views.Length; i++)
        {
            if (views[i] == null)
            {
                views[i] = Instantiate(viewPrefab, transform);
            }
            views[i].Set(this, i);
        }
    }

    [ContextMenu("Open")]
    public void Open()
    {
        gameObject.SetActive(true);
    }

    [ContextMenu("Close")]
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void TryEvolve(int to)
    {
        if (currentNode.TryEvolve(to))
        {
            currentNode = treeNodes[to]; // 進化に成功したら現在のノードを更新
            GetComponentInParent<UnitBase>().Evolve(currentNode.SpeciePrefab);
        }
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

    public bool TryEvolve(int nodeIndex)
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