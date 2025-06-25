using UnityEngine;

public class HitState : BaseState
{
    public override void Enter(StateManager stateManager)
    {
        Debug.Log("Entered Hit");
        stateManager.animator.SetTrigger("Hit");
    }
    public override void Execute(StateManager stateManager) { }
    public override void Exit(StateManager stateManager) { }
    public override void OnCollisionEnter(StateManager stateManager, Collision collision) { }
}
