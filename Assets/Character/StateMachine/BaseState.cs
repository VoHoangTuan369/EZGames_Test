using UnityEngine;

public abstract class BaseState
{
    public abstract void Enter(StateManager stateManager);
    public abstract void Execute(StateManager stateManager);
    public abstract void Exit(StateManager stateManager);
    public abstract void OnCollisionEnter(StateManager stateManager, Collision collision);
}
