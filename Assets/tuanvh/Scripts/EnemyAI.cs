using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Character player;
    public Character enemy;
    private float timer;
    private float interval = 1.5f;

    private void Start()
    {
        //enemy.StateMachine.CurrentState.OnStateExit += OnEnemyStateExit;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            ReactToPlayerState();
        }
    }

    private void OnDisable()
    {
       //enemy.StateMachine.CurrentState.OnStateExit -= OnEnemyStateExit;
    }

    void ReactToPlayerState()
    {

        if (player.StateMachine.CurrentState is IdleState)
        {
            Debug.Log("Player đứng im → Enemy tấn công");
            int attackDir = Random.Range(0, 4);
            enemy.StateMachine.ChangeState(new AttackState(attackDir, enemy.HitCharacters));

        }
        else if (player.StateMachine.CurrentState is AttackState)
        {
            int decision = Random.Range(0, 2);
            if (decision == 0)
            {
                Debug.Log("Player đang tấn công → Enemy né");
                enemy.StateMachine.ChangeState(new DodgeState(0));
            }
            else
            {
                Debug.Log("Player đang tấn công → Enemy đứng yên");
                // Giữ nguyên state hoặc vào IdleState nếu có
            }
        }
    }

    
}
