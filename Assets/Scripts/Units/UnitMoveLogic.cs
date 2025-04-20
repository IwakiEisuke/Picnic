using System;
using System.Collections.Generic;
using System.Linq;

public class FSM
{
    private Dictionary<IState, List<Transition>> _states = new();

    private IState currentState;

    public FSM(Dictionary<IState, List<Transition>> states, int initStateIndex = 0)
    {
        _states = states;
        Next(initStateIndex);
    }

    public void Next(int stateIndex)
    {
        currentState?.Exit();
        currentState = _states.ElementAt(stateIndex).Key;
        currentState.Enter();
    }

    public void Update()
    {
        foreach (var transition in _states[currentState])
        {
            if (transition.condition.Invoke())
            {
                Next(transition.to);
            }
        }

        currentState.Update();
    }

    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }

    public class Transition
    {
        public int to;
        public Func<bool> condition;

        public Transition(int to, Func<bool> condition)
        {
            this.to = to;
            this.condition = condition;
        }
    }
}