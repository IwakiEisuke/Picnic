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

        Evolve();

        return true;

        void Evolve()
        {
            // 進化後のユニットを生成し、元のユニットを破棄
            var evolved = Object.Instantiate(currentNode.SpeciePrefab, owner.transform.position, owner.transform.rotation);
            Object.Destroy(owner.gameObject);

            // 状態異常の引き継ぎ

            // 進化状態の引き継ぎ
            if (owner.TryGetComponent<EvolutionTreeManager>(out var evo))
            {
                evolved.GetComponent<EvolutionTreeManager>().Copy(evo);
            }
        }
    }
}
