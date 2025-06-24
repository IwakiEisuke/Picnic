using System.Collections.Generic;
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
    // 現在実行中のアクション
    List<ExecutingAction> _executingActions = new();

    float remainCoolTime;

    public bool DebugMode => _debugMode;
    public void SetActions(ActionBase[] actions)
    {
        _actionAssets = actions;
    }

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
        var dt = Time.deltaTime;
        UpdateExecutingActions(_executingActions, dt);

        if (remainCoolTime > 0)
        {
            remainCoolTime -= dt;
            
            return;
        }

        EvaluateInvokeAction();
    }

    private void UpdateExecutingActions(List<ExecutingAction> executingActions, float dt)
    {
        for (int i = 0; i < executingActions.Count; i++)
        {
            var executing = executingActions[i];

            executing.remainDuration -= dt;
            executing.remainCooldownTime -= dt;

            if (executing.remainCooldownTime > 0)
            {
                // アクション持続中はUpdateを呼び出す
                if (executing.remainDuration > 0)
                {
                    executing.action.Update();
                }
                continue;
            }

            // アクションループ判定
            if (executing.remainLoopCount > 0 && executing.remainLoopInterval > 0)
            {
                executing.remainLoopInterval -= dt;
                if (executing.remainLoopInterval <= 0 && executing.remainLoopCount > 0)
                {
                    executing.remainLoopCount -= 1;
                    executing.remainLoopInterval = executing.loopInterval;
                    executing.action.Execute();
                }
                continue;
            }

            // 残りループが無い場合、実行中のアクションから削除
            executingActions.RemoveAt(i);
            i -= 1;

            if (_debugMode)
            {
                Debug.Log($"Action {executing.action.name} finished.");
            }
        }
    }

    private void EvaluateInvokeAction()
    {
        var maxScore = -float.Epsilon;
        var actionIndex = -1;

        for (int i = 0; i < _actions.Length; i++)
        {
            var action = _actions[i];
            
            if (_executingActions.Any(x => x.action == action))
            {
                continue;
            }

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
            remainCoolTime = result.delay;
            _executingActions.Add(new ExecutingAction(result));
            if (_debugMode) Debug.Log($"Execute {_actions[actionIndex].name}");
        }
    }
}

public class ExecutingAction
{
    public ActionBase action;
    public float cooldownTime;
    public float duration;
    public int loopCount;
    public float loopInterval;
    public float remainCooldownTime;
    public float remainDuration;
    public int remainLoopCount;
    public float remainLoopInterval;

    public ExecutingAction(ActionExecuteInfo info)
    {
        action = info.action;
        cooldownTime = info.cooldownTime;
        duration = info.duration;
        loopCount = info.loopCount;
        loopInterval = info.loopInterval;
        remainCooldownTime = cooldownTime;
        remainDuration = duration;
        remainLoopCount = loopCount;
        remainLoopInterval = loopInterval;
    }

    public ExecutingAction(ActionBase action, float cooldownTime, float duration, int loopCount, float loopInterval)
    {
        this.action = action;
        this.cooldownTime = cooldownTime;
        this.duration = duration;
        this.loopCount = loopCount;
        this.loopInterval = loopInterval;
        remainCooldownTime = cooldownTime;
        remainDuration = duration;
        remainLoopCount = loopCount;
        remainLoopInterval = loopInterval;
    }
}