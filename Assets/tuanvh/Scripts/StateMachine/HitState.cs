using System.Collections;
using UnityEngine;

public class HitState : BaseState
{
    private float hit_ID;
    public HitState(float id)
    {
        hit_ID = id;
    }
    public override void Enter(StateMachine stateMachine)
    {
        //Debug.Log("Entered Hit");
        base.Enter(stateMachine);
        stateMachine.Animator.SetFloat("Hit_ID", hit_ID);
        stateMachine.Animator.SetTrigger("Hit");
        stateMachine.Invoke("TransitionToIdle", 1.0f);
    }
    public override void Exit(StateMachine stateMachine) {
        base.Exit(stateMachine);
        //stateMachine.ChangeState(new IdleState());
    }


}
