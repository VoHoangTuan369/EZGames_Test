using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;
    public Type type;

    public StateManager stateManager;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"takes {damage} damage! Remaining HP: {health}");
        stateManager.ChangeState(new HitState());

        if (health <= 0)
        {
            Debug.Log(type + " defeated!");
            // Gọi anim chết, disable điều khiển, v.v.
        }
    }
}
public enum Type 
{
    Player, Enemy
}

