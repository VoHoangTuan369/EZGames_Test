using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHit : MonoBehaviour
{
    public StateManager stateManager;
    public Type type;
    private void Start()
    {
        type = GetComponentInParent<Health>().type;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (type) 
        {
            case Type.Player:
                if (other.CompareTag("Enemy"))
                {
                    Debug.Log("Player Punch landed → Enemy takes damage!");
                    // Gọi hàm gây sát thương
                    other.transform.parent.GetComponent<Health>()?.TakeDamage(10);
                }
                break;
            case Type.Enemy:
                if (other.CompareTag("Player"))
                {
                    Debug.Log("Enemy Punch landed → Player takes damage!");
                    // Gọi hàm gây sát thương
                    other.transform.parent.GetComponent<Health>()?.TakeDamage(10);
                }
                break;
        }
    }
}
