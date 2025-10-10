using UnityEngine;

/// <summary>
/// ユニット毎の進化ツリーの管理を行うクラス
/// </summary>
public class EvolutionTreeManager : MonoBehaviour
{
    [SerializeField] UnitBase initOwner;
    [SerializeField] EvolutionTree evolutionTreeAsset;

    RuntimeEvolutionTree runtimeEvolutionTree;

    public RuntimeEvolutionTree EvolutionTree => runtimeEvolutionTree;

    private void Start()
    {
        if (runtimeEvolutionTree == null && evolutionTreeAsset != null)
        {
            runtimeEvolutionTree = new();
            runtimeEvolutionTree.SetTree(evolutionTreeAsset);

            if (initOwner)
            {
                runtimeEvolutionTree.SetOwner(initOwner);
            }
            else
            {
                Debug.LogWarning("EvolutionTreeView: Owner is not set. Please assign a UnitBase to the owner field.");
            }
        }
    }

    // 他のEvolutionTreeManagerから進化ツリーのデータをコピーします。
    public void Copy(EvolutionTreeManager origin)
    {
        origin.runtimeEvolutionTree.SetOwner(initOwner);
        runtimeEvolutionTree = origin.runtimeEvolutionTree;
    }
}
