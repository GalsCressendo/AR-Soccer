using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMazeController : MonoBehaviour
{
    NavMeshAgent agent;
    const string BALL_TAG = "Ball";
    const string GOAL_TARGET = "Goal_Red";
    const string RUNNING_ANIM_PARAM = "isRunning";
    Transform ballTarget = null;
    Animator animator;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag(BALL_TAG) && !GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Ball>().isAttached)
        {
            animator.SetBool(RUNNING_ANIM_PARAM, true);
            ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
            if (!ballTarget.GetComponent<Ball>().isAttached)
            {
                agent.SetDestination(ballTarget.position);
                if(Vector3.Distance(transform.position, ballTarget.position) < 0.001f)
                {
                    ballTarget.GetComponent<Ball>().isAttached = true;
                    Debug.Log("Have Arrived");
                }
            }

        }
    }
}
