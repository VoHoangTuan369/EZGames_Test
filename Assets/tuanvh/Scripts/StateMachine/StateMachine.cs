using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    private string stateName;
    public BaseState CurrentState;
    public Animator Animator;
    
    public StateMachine(){}
    
    public StateMachine(Animator animator)
    {
        this.Animator = animator;
    }

    private void Update()
    {
        CurrentState?.Execute(this);
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
        stateName = newState.ToString();
        CurrentState.Enter(this);
    }
}
