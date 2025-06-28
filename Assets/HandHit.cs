using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHit : MonoBehaviour
{
    public StateMachine stateMachine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Punch landed → Enemy takes damage!");
            // Gọi hàm gây sát thương
            other.transform.parent.GetComponent<EnemyHealth>()?.TakeDamage(10);
        }
    }
}
