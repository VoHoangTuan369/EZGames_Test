using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHit : MonoBehaviour
{
    public StateManager stateManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Punch landed → Enemy takes damage!");
            // Gọi hàm gây sát thương
            other.GetComponent<EnemyHealth>()?.TakeDamage(10);
        }
    }
}
