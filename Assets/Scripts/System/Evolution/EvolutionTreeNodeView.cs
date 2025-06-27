using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �i���c���[�̊e�m�[�h�Ƃ��̃G�b�W��\�����邽�߂̃r���[�R���|�[�l���g
/// </summary>
public class EvolutionTreeNodeView : MonoBehaviour
{
    EvolutionTreeNode node;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image icon;
    [SerializeField] Image edgeArrowPref;
    [SerializeField] Button iconButton;

    /// <summary>
    /// �c���[���ƕ\������m�[�h�̃C���f�b�N�X����m�[�h�P�ʂŃr���[���쐬���܂�
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="index"></param>
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

            arrow.rectTransform.anchoredPosition = mid - rectTransform.anchoredPosition;
            arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angleY);
        }

        iconButton.onClick.AddListener(() => tree.TryEvolve(index));
    }
}
