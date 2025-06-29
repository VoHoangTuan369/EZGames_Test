using UnityEngine;

public class DodgeState : BaseState
{
    private float dodge_ID;
    public DodgeState(float id)
    {
        dodge_ID = id;
    }
    public override void Enter(StateMachine stateMachine)
    {
        //Debug.Log("Entered Dodge");
        base.Enter(stateMachine);
        stateMachine.Animator.SetFloat("Dodge_ID", dodge_ID);
        stateMachine.Animator.SetTrigger("Dodge");
        stateMachine.Invoke("TransitionToIdle", 1.0f);
    }
    public override void Exit(StateMachine stateMachine) {
        base.Exit(stateMachine);
        //stateMachine.ChangeState(new IdleState());
    }
}
