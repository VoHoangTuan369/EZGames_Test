using System;
using UnityEngine;

public class BaseState
{
    public Action OnStateEnter;
    public Action OnStateExit;
    public virtual void Enter(StateMachine stateMachine)
    {
        OnStateEnter?.Invoke();
    }
    public virtual void Exit(StateMachine stateMachine)
    {
        OnStateExit?.Invoke();
    }
}
