using UnityEngine;

public class IdleState : BaseState
{
    public override void Enter(StateMachine stateMachine)
    {
        Debug.Log("Entered Idle");
        //stateMachine.animator.SetTrigger("Idle");
    }
    public override void Execute(StateMachine stateMachine) { }

    public override void Exit(StateMachine stateMachine)
    {
        Debug.Log("Exit Idle");
    }

}
