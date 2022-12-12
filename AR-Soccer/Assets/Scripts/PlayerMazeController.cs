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
        if (ballTarget == null)
        {
            if (GameObject.FindGameObjectWithTag(BALL_TAG))
            {
                ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
            }
        }
        else if (ballTarget != null && !ballTarget.GetComponent<Ball>().isAttached)
        {
            animator.SetBool(RUNNING_ANIM_PARAM, true);
            agent.SetDestination(ballTarget.position);
            if (Vector3.Distance(transform.position, ballTarget.position) < 0.05f)
            {
                ballTarget.GetComponent<Ball>().isAttached = true;
                ballTarget.transform.SetParent(gameObject.transform, true);
                Debug.Log("Have Arrived");
            }
        }
        else if (ballTarget != null && ballTarget.GetComponent<Ball>().isAttached)
        {
            agent.SetDestination(GameObject.FindGameObjectWithTag(GOAL_TARGET).transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GOAL_TARGET)
        {
            Debug.Log("PLAYER WIN!!!");
            Destroy(gameObject);
        }
    }
}
