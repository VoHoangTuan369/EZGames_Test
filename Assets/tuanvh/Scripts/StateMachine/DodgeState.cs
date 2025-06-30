using UnityEngine;

public class DodgeState : BaseState
{
    public int DodgeID;

    public override void Enter(StateMachine stateMachine)
    {
        //Debug.Log("Entered Dodge");
        base.Enter(stateMachine);
        stateMachine.Animator.SetFloat("Dodge_ID", DodgeID);
        stateMachine.Animator.SetTrigger("Dodge");
    }
    public override void Exit(StateMachine stateMachine) {
        base.Exit(stateMachine);
        //stateMachine.ChangeState(new IdleState());
    }
}
