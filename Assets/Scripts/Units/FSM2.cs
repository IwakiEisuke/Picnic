using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 遷移を外部からのみ扱うFSM
/// </summary>
public class FSM2<T> where T : Enum
{
    private readonly Dictionary<T, FSM.IState> _states = new();
    private FSM.IState _currentState;

    public FSM2(Dictionary<T, FSM.IState> states, int initStateIndex = 0)
    {
        _states = states;
        Next(initStateIndex);
    }

    private void Next(int stateIndex)
    {
        _currentState?.Exit();
        _currentState = _states.ElementAt(stateIndex).Value;
        _currentState.Enter();
    }

    public void Next(T state)
    {
        _currentState?.Exit();
        _currentState = _states[state];
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState.Update();
    }
}