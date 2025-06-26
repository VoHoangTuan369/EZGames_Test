using UnityEngine;

public class AttackState : BaseState
{
    private float attackID;

    public AttackState(float id)
    {
        Debug.Log("id: " + id);
        attackID = id;
    }

    public override void Enter(StateManager stateManager)
    {
        Debug.Log("Entered Attack - ID: " + attackID);
        stateManager.animator.SetFloat("Attack_ID", attackID);
        stateManager.animator.SetTrigger("Attack");
        stateManager.bodyCollider.isTrigger = false;
    }
    public override void Execute(StateManager stateManager) { }
    public override void Exit(StateManager stateManager) 
    {
        stateManager.bodyCollider.isTrigger = true;
    }
    public override void OnCollisionEnter(StateManager stateManager, Collision collision) { }
}
