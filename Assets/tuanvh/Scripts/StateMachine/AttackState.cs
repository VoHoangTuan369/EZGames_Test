using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float attackID;
    
    private List<Character> hitCharacters;

    public AttackState(float id, List<Character> hitCharacters)
    {
        Debug.Log("id: " + id);
        attackID = id;
        this.hitCharacters = hitCharacters;
    }

    public override void Enter(StateMachine stateMachine)
    {
        //Debug.Log("Entered Attack - ID: " + attackID);
        base.Enter(stateMachine);
        stateMachine.Animator.SetFloat("Attack_ID", attackID);
        stateMachine.Animator.SetTrigger("Attack");

        stateMachine.Invoke("TransitionToIdle", 1.0f);
    }

    public override void Exit(StateMachine stateMachine)
    {
        Debug.Log("Exit Attack");
        base.Exit(stateMachine);
        hitCharacters.Clear();
        //stateMachine.ChangeState(new IdleState());
    }
}
