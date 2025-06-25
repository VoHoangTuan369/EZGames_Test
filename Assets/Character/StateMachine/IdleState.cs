using UnityEngine;

public class IdleState : BaseState
{
    public override void Enter(StateManager stateManager)
    {
        Debug.Log("Entered Idle");
        stateManager.animator.SetTrigger("Idle");
    }
    public override void Execute(StateManager stateManager) { }
    public override void Exit(StateManager stateManager) { }
    public override void OnCollisionEnter(StateManager stateManager, Collision collision) { }
}
