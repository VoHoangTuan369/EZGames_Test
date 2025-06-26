using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 3f;
    public float decisionInterval = 2f;
    public StateManager stateManager;
    public StateManager playerStateManager;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= decisionInterval)
        {
            timer = 0;
            MakeDecision();
        }
    }

    void MakeDecision()
    {
        if (playerStateManager == null || playerStateManager.currentState == null)
            return;

        bool playerIsAttacking = playerStateManager.currentState is AttackState;
        bool playerIsIdle = !(playerIsAttacking || playerStateManager.currentState is DefendState);

        int decision = Random.Range(0, 2); // 0 = idle, 1 = react

        if (playerIsAttacking)
        {
            if (decision == 0)
            {
                Debug.Log("Enemy stays idle when player attacks");
                // Có thể chuyển sang IdleState nếu có
            }
            else
            {
                Debug.Log("Enemy defends against attack");
                stateManager.ChangeState(new DefendState());
            }
        }
        else if (playerIsIdle)
        {
            if (decision == 0)
            {
                Debug.Log("Enemy remains idle");
                // Idle or patrol
            }
            else
            {
                int attackDir = Random.Range(0, 4);
                Debug.Log($"Enemy takes initiative to attack in direction {attackDir}");
                stateManager.ChangeState(new AttackState(attackDir));
            }
        }
    }
}
