using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Health scriptHealth;
    [SerializeField] HandHit leftHandHit;
    [SerializeField] HandHit rightHandHit;

    private void Start()
    {
        //leftHandHit.type = scriptHealth.type;
        //rightHandHit.type = scriptHealth.type;
    }
}
