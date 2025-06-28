using System;
using UnityEngine;

public abstract class BaseState
{
    public abstract void Enter(StateMachine stateMachine);
    public abstract void Execute(StateMachine stateMachine);
    public abstract void Exit(StateMachine stateMachine);
}
