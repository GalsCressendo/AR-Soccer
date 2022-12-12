using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMazeController : MonoBehaviour
{
    NavMeshAgent agent;
    const string BALL_TAG = "Ball";
    const string GOAL_TARGET = "Goal_Red";
    Transform ballTarget = null;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag(BALL_TAG))
        {
            ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
            if (!ballTarget.GetComponent<Ball>().isAttached)
            {
                agent.SetDestination(ballTarget.position);
                if(Vector3.Distance(transform.position, ballTarget.position) < 0.001f)
                {
                    Debug.Log("Have Arrived");
                }
            }

        }
    }
}
