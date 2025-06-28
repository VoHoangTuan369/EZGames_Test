using UnityEngine;

public class DodgeState : BaseState
{
    public override void Enter(StateMachine stateMachine)
    {
        Debug.Log("Entered Dodge");
        stateMachine.animator.SetTrigger("Dodge");
    }
    public override void Execute(StateMachine stateMachine) { }
    public override void Exit(StateMachine stateMachine) { }
}
