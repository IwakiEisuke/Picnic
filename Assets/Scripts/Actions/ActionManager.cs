using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 最も評価の高いアクションを実行するためのマネージャー
/// </summary>
public class ActionManager : MonoBehaviour
{
    [SerializeField] bool _debugMode = false;
    [SerializeField] UnitBase _unitBase;
    [SerializeField, FormerlySerializedAs("_actions")] ActionBase[] _actionAssets;

    // このコンポーネントで操作するActionBaseを格納する配列
    ActionBase[] _actions;

    ActionBase _currentAction;

    float remainInterval;
    float remainLoopInterval;
    int remainLoopCount;
    float currentLoopInterval;

    private void Start()
    {
        // コンポーネントに個別なScriptableObjectを生成
        _actions = _actionAssets.Select(x => ScriptableObjectUtilityRuntime.Clone(x)).ToArray();

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
        if (remainLoopCount > 0 && remainLoopInterval > 0)
        {
            remainLoopInterval -= Time.deltaTime;
            if (remainLoopInterval <= 0 && remainLoopCount > 0)
            {
                remainLoopCount--;
                remainLoopInterval = currentLoopInterval;
                _currentAction.Execute();
            }
            return;
        }

        if (remainInterval > 0)
        {
            remainInterval -= Time.deltaTime;
            _currentAction.Update();
            return;
        }

        EvaluateInvokeAction();
    }

    private void EvaluateInvokeAction()
    {
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
            _currentAction = _actions[actionIndex];
            remainLoopCount = result.loopCount;
            remainLoopInterval = result.loopInterval;
            currentLoopInterval = result.loopInterval;
            if (_debugMode) Debug.Log($"Execute {_actions[actionIndex].name}");
        }
    }
}
