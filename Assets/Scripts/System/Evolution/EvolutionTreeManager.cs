using UnityEngine;

/// <summary>
/// ユニット毎の進化ツリーの管理を行うクラス
/// </summary>
public class EvolutionTreeManager : MonoBehaviour
{
    [SerializeField] UnitBase initOwner;
    [SerializeField] EvolutionTree evolutionTreeAsset;

    RuntimeEvolutionTree evolutionTree;

    public RuntimeEvolutionTree EvolutionTree => evolutionTree;

    private void Start()
    {
        if (evolutionTree == null && evolutionTreeAsset != null)
        {
            evolutionTree = new();
            evolutionTree.SetTree(evolutionTreeAsset);

            if (initOwner)
            {
                evolutionTree.SetOwner(initOwner);
            }
            else
            {
                Debug.LogWarning("EvolutionTreeView: Owner is not set. Please assign a UnitBase to the owner field.");
            }
        }
    }

    public void Copy(EvolutionTreeManager origin)
    {
        origin.evolutionTree.SetOwner(initOwner);
        evolutionTree = origin.evolutionTree;
    }
}
