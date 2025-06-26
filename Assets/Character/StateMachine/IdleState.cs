using UnityEngine;

public class IdleState : BaseState
{
    public override void Enter(StateManager stateManager)
    {
        Debug.Log("Entered Idle");
        stateManager.animator.SetTrigger("Idle");
        stateManager.leftHandCollider.isTrigger = false;
        stateManager.rightHandCollider.isTrigger = false;
    }
    public override void Execute(StateManager stateManager) { }
    public override void Exit(StateManager stateManager) 
    {
        stateManager.leftHandCollider.isTrigger = true;
        stateManager.rightHandCollider.isTrigger = true;
    }
    public override void OnCollisionEnter(StateManager stateManager, Collision collision) { }
}
