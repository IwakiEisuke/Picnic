using System;
using UnityEngine;

public class EvolutionTree : MonoBehaviour
{
    [SerializeField] EvolutionTreeNode[] treeNodes;
    [SerializeField] EvolutionTreeNodeView viewPrefab;
    [SerializeField] Sprite edgeArrow;
    [SerializeField] EvolutionTreeNodeView[] views;

    public EvolutionTreeNode[] TreeNodes => treeNodes;
}

[Serializable]
public class EvolutionTreeNode
{
    [SerializeField] string specieName;
    [SerializeField] string description;
    [SerializeField] EvolutionTreeEdge[] edges;
    [SerializeField] GameObject speciePrefab;
    [SerializeField] Vector2 pos;
    [SerializeField] Sprite icon;
    [SerializeField] bool isUnlocked;

    public string SpecieName => specieName;
    public string Description => description;
    public EvolutionTreeEdge[] Edges => edges;
    public Vector2 Position => pos;
    public Sprite Icon => icon;
    public bool IsUnlocked => isUnlocked;

    public bool CanEvolve(int nodeIndex)
    {
        for (int i = 0; i < edges.Length; i++)
        {
            if (edges[i].ToIndex == nodeIndex)
            {
                return edges[i].Cost <= GameManager.Inventory.HoneyCount;
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