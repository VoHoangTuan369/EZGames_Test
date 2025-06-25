using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Enemy takes {damage} damage! Remaining HP: {health}");

        if (health <= 0)
        {
            Debug.Log("Enemy defeated!");
            // Gọi anim chết, disable điều khiển, v.v.
        }
    }
}
