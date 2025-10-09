using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionTree", menuName = "EvolutionTree", order = 1)]
public class EvolutionTree : ScriptableObject
{
    [SerializeField] EvolutionTreeNode[] treeNodes;

    public EvolutionTreeNode[] TreeNodes => treeNodes;
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

                Debug.LogWarning("EvolutionTree: 進化に失敗しました（進化コスト分の資源を持っていません）");
                return false; // コストが足りない場合は進化できない
            }
        }

        Debug.LogWarning("EvolutionTree: 進化に失敗しました（現在ノードから選択されたノードへ到達できません）");
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