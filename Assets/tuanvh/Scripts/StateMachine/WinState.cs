using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : BaseState
{
    public override void Enter(StateMachine stateMachine)
    { ;
        stateMachine.Animator.SetTrigger("Win");

        //stateMachine.Invoke("TransitionToIdle", 1.0f);
    }
    public override void Exit(StateMachine stateMachine)
    {
    }
}
