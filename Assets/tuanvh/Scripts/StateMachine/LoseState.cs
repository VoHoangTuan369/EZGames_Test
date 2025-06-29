using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : BaseState
{
    public override void Enter(StateMachine stateMachine)
    {
        base.Enter(stateMachine);
        stateMachine.Animator.SetTrigger("Lose");
        Debug.Log("Losessss");
        //stateMachine.Invoke("TransitionToIdle", 1.0f);
    }
    public override void Exit(StateMachine stateMachine)
    {
        base.Exit(stateMachine);
    }
}
