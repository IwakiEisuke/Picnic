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
        currentState = _states.ElementAt(initStateIndex).Key;
    }

    public void Next(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        foreach (var transition in _states[currentState])
        {
            if (transition.condition.Invoke())
            {
                currentState.Exit();
                currentState = _states.ElementAt(transition.to).Key;
                currentState.Enter();
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