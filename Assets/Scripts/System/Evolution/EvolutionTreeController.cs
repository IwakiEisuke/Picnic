using UnityEngine;

/// <summary>
/// ユニット毎の進化ツリーの管理を行うクラス
/// </summary>
public class EvolutionTreeController : MonoBehaviour
{
    [SerializeField] UnitBase owner;
    [SerializeField] EvolutionTree evolutionTreeAsset;

    EvolutionTree evolutionTree;

    public EvolutionTree EvolutionTree => evolutionTree;

    private void Start()
    {
        if (evolutionTree == null && evolutionTreeAsset != null)
        {
            evolutionTree = ScriptableObject.CreateInstance<EvolutionTree>();
            evolutionTree.Copy(evolutionTreeAsset);
            if (owner)
            {
                evolutionTree.SetOwner(owner);
            }
            else
            {
                Debug.LogWarning("EvolutionTreeView: Owner is not set. Please assign a UnitBase to the owner field.");
            }
        }
    }

    public void Copy(EvolutionTreeController origin)
    {
        evolutionTree = origin.evolutionTree;
    }
}
