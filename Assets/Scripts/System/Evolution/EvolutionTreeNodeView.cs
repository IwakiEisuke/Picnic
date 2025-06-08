using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionTreeNodeView : MonoBehaviour
{
    EvolutionTreeNode node;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image icon;
    [SerializeField] Image edgeArrowPref;

    public void Set(EvolutionTree tree, int index)
    {
        node = tree.TreeNodes[index];
        icon.sprite = node.Icon;
        rectTransform.anchoredPosition = node.Position;
        foreach (var child in node.Edges)
        {
            var dir = (tree.TreeNodes[child.ToIndex].Position - node.Position).normalized;
            var angleY = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            var mid = (node.Position + tree.TreeNodes[child.ToIndex].Position) / 2;

            var arrow = Instantiate(edgeArrowPref, transform);

            arrow.rectTransform.anchoredPosition = mid;
            arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angleY);
        }
    }
}
