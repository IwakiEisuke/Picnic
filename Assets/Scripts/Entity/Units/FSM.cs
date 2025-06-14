using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 遷移ごと組み込んだFSM
/// </summary>
public class FSM
{
    private Dictionary<FSMState, List<Transition>> _states = new();

    private FSMState currentState;

    public FSM(Dictionary<FSMState, List<Transition>> states, int initStateIndex = 0)
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

public class FSMState
{
    protected readonly UnitBase _parent;
    protected readonly UnitStats _stats;
    protected readonly AttackCollisionController _collisionController;
    protected readonly NavMeshAgent _agent;

    public FSMState(UnitBase parent)
    {
        _parent = parent;
        _stats = parent.Stats;
        _agent = parent.Agent;
        _collisionController = parent.CollisionController;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

    protected void Log(string message)
    {
        Debug.Log($"<{_parent.name}>: {message}");
    }
}