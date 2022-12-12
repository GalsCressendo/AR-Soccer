using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMazeController : MonoBehaviour
{
    NavMeshAgent agent;
    const string BALL_TAG = "Ball";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag(BALL_TAG) && !GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Ball>().isAttached)
        {
            Transform ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
            agent.SetDestination(ballTarget.position);
        }
    }
}
