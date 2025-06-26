using UnityEngine;

public class HitState : BaseState
{
    public override void Enter(StateManager stateManager)
    {
        Debug.Log("Entered Hit");
        stateManager.animator.SetTrigger("Hit");
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
