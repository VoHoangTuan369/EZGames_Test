using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public BaseState currentState;
    public Animator animator;
    public Collider leftHandCollider;
    public Collider rightHandCollider;
    public Collider bodyCollider;

    void Start()
    {
        ChangeState(new IdleState());
    }

    void Update()
    {
        currentState?.Execute(this);
    }

    public void ChangeState(BaseState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        currentState?.OnCollisionEnter(this, collision);
    }
}
