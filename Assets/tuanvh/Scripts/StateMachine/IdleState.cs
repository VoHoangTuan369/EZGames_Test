using UnityEngine;

public class IdleState : BaseState
{
    public override void Enter(StateMachine stateMachine)
    {
        base.Enter(stateMachine);
        Debug.Log("Entered Idle");
        stateMachine.Animator.speed = 1f;
    }

    public override void Exit(StateMachine stateMachine)
    {
        base.Exit(stateMachine);
        Debug.Log("Exit Idle");
    }

}
