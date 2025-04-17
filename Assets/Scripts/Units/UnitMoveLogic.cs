using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitMoveLogicBase
{
    protected NavMeshAgent _agent;
    protected UnitStats _stats;
    protected Collider[] _hits = new Collider[1];

    protected UnitBase parent;
    protected bool isInitialized;

    public void Init(UnitBase parent)
    {
        this.parent = parent;
        _agent = parent.GetComponent<NavMeshAgent>();
        _stats = parent.Stats;
        isInitialized = true;
    }

    protected bool IsInitialized()
    {
        if (isInitialized)
        {
            return true;
        }

        Debug.Log("Is not Initialized");
        return false;
    }

    public void Attack()
    {
        Debug.Log($"{_agent.name}: Attack");
        foreach (var damageable in _hits[0].GetComponentsInParent<IDamageable>())
        {
            damageable.TakeDamage(_stats);
        }
    }

    public bool CheckAround(LayerMask layerMask)
    {
        return Physics.OverlapSphereNonAlloc(_agent.transform.position, _stats.AttackRadius, _hits, layerMask.value) > 0;
    }

    public abstract void Start();
}

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