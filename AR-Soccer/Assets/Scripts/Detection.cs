using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public GameObject unit;

    const string ATTACKER_TAG = "Attacker";

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == ATTACKER_TAG)
        {
            unit.GetComponent<Defender>().ChaseAttacker(other.transform);
        }
    }
}
