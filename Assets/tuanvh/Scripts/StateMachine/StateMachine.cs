using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private BaseState currentState;
    public Animator animator;
    
    public StateMachine(){}
    
    public StateMachine(Animator animator)
    {
        this.animator = animator;
    }

    public void InitializeState(BaseState initialState)
    {
        currentState = initialState;
        initialState.Enter(this);
    }

    public void ChangeState(BaseState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }
}
