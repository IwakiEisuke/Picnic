using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class RuntimeEvolutionTree
{
    EvolutionTree _tree;
    EvolutionTreeNode currentNode;
    UnitBase owner;

    public ReadOnlyArray<EvolutionTreeNode> TreeNodes => _tree.TreeNodes;

    public void SetTree(EvolutionTree tree)
    {
        _tree = tree;
        currentNode = _tree.TreeNodes[0];
    }

    public void SetOwner(UnitBase owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// 指定したノードに進化を試みます。
    /// </summary>
    /// <param name="to"></param>
    public bool TryEvolve(int to)
    {
        if (owner == null) // 進化させる対象となるオーナーが設定されていない場合は終了
        {
            Debug.LogWarning("EvolutionTree: Owner is not set. Please assign a UnitBase to the owner field.");
            return false;
        }

        if (!currentNode.CanEvolve(to)) // 進化に必要な条件を満たしていない場合は終了
        {
            return false;
        }

        currentNode = _tree.TreeNodes[to]; // 進化に成功したら現在のノードを更新

        return TryEvolveUnit();

        bool TryEvolveUnit()
        {
            // 進化後のユニットを生成し、元のユニットを破棄
            var evolved = Object.Instantiate(currentNode.SpeciePrefab, owner.transform.position, owner.transform.rotation);

            var oldEvo = owner.GetComponent<EvolutionTreeManager>();
            var newEvo = evolved.GetComponent<EvolutionTreeManager>();

            // 進化元・進化先のユニットにEvolutionTreeManagerが存在しない場合は終了
            if (oldEvo == null)
            {
                Debug.LogWarning("EvolutionTree: 進化に失敗しました（進化元のユニットにEvolutionTreeManagerが存在しません）");
                Object.Destroy(evolved.gameObject);
                return false;
            }
            else if (newEvo == null)
            {
                Debug.LogWarning("EvolutionTree: 進化に失敗しました（進化先のユニットにEvolutionTreeManagerが存在しません）");
                Object.Destroy(evolved.gameObject);
                return false;
            }
            else
            {
                Object.Destroy(oldEvo.gameObject);
            }

            // 状態異常の引き継ぎが必要な場合はここで実装

            // 進化状態の引き継ぎ
            newEvo.Copy(oldEvo);

            return true;
        }
    }
}
