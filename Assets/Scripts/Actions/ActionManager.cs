using UnityEngine;

/// <summary>
/// 最も評価の高いアクションを実行するためのマネージャー
/// </summary>
public class ActionManager : MonoBehaviour
{
    [SerializeField] UnitBase _unitBase;
    [SerializeField] ActionBase[] _actions;

    private void Start()
    {
        foreach (var action in _actions)
        {
            if (action != null)
            {
                action.Initialize(_unitBase);
            }
        }
    }

    private void Update()
    {
        var maxScore = 0f;
        var actionIndex = -1;

        for (int i = 0; i < _actions.Length; i++)
        {
            var action = _actions[i];
            if (action == null) continue;
            float score = action.Evaluate();
            if (score > maxScore)
            {
                maxScore = score;
                actionIndex = i;
            }
        }

        if (actionIndex >= 0)
        {
            _actions[actionIndex].Execute();
        }
    }
}
