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
    [SerializeField] int[] childIndex;
    [SerializeField] GameObject speciePrefab;
    [SerializeField] Vector2 pos;
    [SerializeField] Sprite icon;
    [SerializeField] bool isUnlocked;

    public string SpecieName => specieName;
    public string Description => description;
    public int[] ChildrenIndex => childIndex;
    public Vector2 Position => pos;
    public Sprite Icon => icon;
    public bool IsUnlocked => isUnlocked;
}