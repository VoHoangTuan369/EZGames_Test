using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState CurrentState;
    public Animator Animator;
    
    public StateMachine(){}
    
    public StateMachine(Animator animator)
    {
        this.Animator = animator;
    }

    public void InitializeState(BaseState initialState)
    {
        CurrentState = initialState;
        initialState.Enter(this);
    }

    public void ChangeState(BaseState newState)
    {
        CurrentState?.Exit(this);
        CurrentState = newState;
        CurrentState.Enter(this);
    }
    public void TransitionToIdle()
    {
        ChangeState(new IdleState());
    }
}
