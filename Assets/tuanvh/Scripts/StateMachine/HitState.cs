using UnityEngine;

public class HitState : BaseState
{
    public override void Enter(StateMachine stateMachine)
    {
        Debug.Log("Entered Hit");
        stateMachine.animator.SetTrigger("Hit");
    }
    public override void Execute(StateMachine stateMachine) { }
    public override void Exit(StateMachine stateMachine) { }

}
