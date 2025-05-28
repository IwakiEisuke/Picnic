using System.Collections.Generic;

/// <summary>
/// �J�ڂ��O������݈̂���FSM
/// </summary>
public class FSM2
{
    private List<FSM.IState> _states = new();
    private FSM.IState _currentState;

    public FSM2(List<FSM.IState> states)
    {
        _states = states;
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
