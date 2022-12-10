using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public GameObject unit;

    const string PLAYER_TAG = "Player";
    const string ENEMY_TAG = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case PLAYER_TAG:
                unit.GetComponent<EnemyDefense>().ChaseAttacker(other.transform);
                break;
        }
    }
}
