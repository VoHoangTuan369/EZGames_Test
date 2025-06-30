using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackState : BaseState
{
    public int AttackID;
    public float Speed;
    
    private bool isAttacking;
    
    public override void Enter(StateMachine stateMachine)
    {
        base.Enter(stateMachine);
        stateMachine.Animator.SetFloat("Attack_ID", AttackID);
        stateMachine.Animator.speed = Speed;
        stateMachine.Animator.SetTrigger("Attack");
    }

    public override void Execute(StateMachine stateMachine)
    {
        base.Execute(stateMachine);
    }

    public override void Exit(StateMachine stateMachine)
    {
        base.Exit(stateMachine);
    }

}