using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float attackID;
    private float speed;
    
    private List<Character> hitCharacters;

    public AttackState(float id, List<Character> hitCharacters, float speed = 1f)
    {
        Debug.Log("id: " + id);
        attackID = id;
        speed = speed;
        this.hitCharacters = hitCharacters;
    }

    public override void Enter(StateMachine stateMachine)
    {
        Debug.Log("Entered Attack - ID: " + attackID);
        stateMachine.animator.SetFloat("Attack_ID", attackID);
        stateMachine.animator.SetTrigger("Attack");
    }
    public override void Execute(StateMachine stateMachine) { }

    public override void Exit(StateMachine stateMachine)
    {
        Debug.Log("Exit Attack");
        hitCharacters.Clear();
    }
}
