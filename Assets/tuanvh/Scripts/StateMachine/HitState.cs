using System.Collections;
using UnityEngine;

public class HitState : BaseState
{
    public int HitID;

    public override void Enter(StateMachine stateMachine)
    {
        //Debug.Log("Entered Hit");
        base.Enter(stateMachine);
        stateMachine.Animator.SetFloat("Hit_ID", HitID);
        stateMachine.Animator.SetTrigger("Hit");
    }
    public override void Exit(StateMachine stateMachine) {
        base.Exit(stateMachine);
        //stateMachine.ChangeState(new IdleState());
    }


}
