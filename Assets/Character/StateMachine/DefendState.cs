using UnityEngine;

public class DefendState : BaseState
{
    public override void Enter(StateManager stateManager)
    {
        Debug.Log("Entered Defend");
        stateManager.animator.SetTrigger("Defend");
    }
    public override void Execute(StateManager stateManager) { }
    public override void Exit(StateManager stateManager) { }
    public override void OnCollisionEnter(StateManager stateManager, Collision collision) { }
}
