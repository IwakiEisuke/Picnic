using System.Collections.Generic;

/// <summary>
/// 遷移を外部からのみ扱うFSM
/// </summary>
public class FSM2
{
    private List<FSM.IState> _states = new();
    private FSM.IState _currentState;

    public FSM2(List<FSM.IState> states, int initStateIndex = 0)
    {
        _states = states;
        Next(initStateIndex);
    }

    public void Next(int stateIndex)
    {
        _currentState?.Exit();
        _currentState = _states[stateIndex];
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState.Update();
    }
}
