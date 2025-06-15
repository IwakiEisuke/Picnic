using UnityEngine;

/// <summary>
/// 最も評価の高いアクションを実行するためのマネージャー
/// </summary>
public class ActionManager : MonoBehaviour
{
    [SerializeField] bool _debugMode = false;
    [SerializeField] UnitBase _unitBase;
    [SerializeField] ActionBase[] _actions;

    float remainInterval;

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
        if (remainInterval > 0)
        {
            remainInterval -= Time.deltaTime;
            return;
        }

        var maxScore = -float.Epsilon;
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
            var result = _actions[actionIndex].Execute();
            remainInterval = result.interval;
            if (_debugMode) Debug.Log($"Execute {_actions[actionIndex].name}");
        }
    }
}
