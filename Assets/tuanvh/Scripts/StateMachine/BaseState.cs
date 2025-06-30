using System;
using UnityEngine;

public class BaseState
{
    public Action OnStateEnter;
    public Action OnStateExecute;
    public Action OnStateExit;
    
    private bool _isEntering = false;
    private bool _isExecuting = false;
    private bool _isExiting = false;
    
    public virtual void Enter(StateMachine stateMachine)
    {
        if (_isEntering) return; // Tránh recursive call
        
        _isEntering = true;
        try
        {
            OnStateEnter?.Invoke();
        }
        finally
        {
            _isEntering = false;
        }
    }

    public virtual void Execute(StateMachine stateMachine)
    {
        if (_isExecuting) return;
        _isExecuting = true;
        try
        {
            OnStateExecute?.Invoke();
        }
        finally
        {
            _isExecuting = false;
        }
    }
    
    public virtual void Exit(StateMachine stateMachine)
    {
        if (_isExiting) return; // Tránh recursive call
        
        _isExiting = true;
        try
        {
            OnStateExit?.Invoke();
        }
        finally
        {
            _isExiting = false;
        }
    }
}

