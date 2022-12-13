using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMazeController : MonoBehaviour
{
    NavMeshAgent agent;
    const string BALL_TAG = "Ball";
    const string RUNNING_ANIM_PARAM = "isRunning";
    const string GOAL_TARGET_TAG = "Goal_Red";
    Transform ballTarget = null;
    Animator animator;
    [SerializeField]MazeGameManager gameManager;
    [SerializeField] GameObject confettiParticle;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameManager.gameIsActive)
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
                }
            }
            else if (ballTarget != null && ballTarget.GetComponent<Ball>().isAttached)
            {
                agent.SetDestination(GameObject.FindGameObjectWithTag(GOAL_TARGET_TAG).transform.position);
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GOAL_TARGET_TAG)
        {
            FindObjectOfType<AudioManager>().PlayAudio(AudioManager.GOAL_SFX);
            gameManager.GetComponent<MazeGameManager>().DeclarePlayerWinner();
            SpawnConfetti();
            Destroy(gameObject);
        }
    }

    private void SpawnConfetti()
    {
        GameObject confetti = Instantiate(confettiParticle, confettiParticle.transform.position, confettiParticle.transform.rotation);
        confetti.GetComponent<Confetti>().GetConffeti(GameObject.FindGameObjectWithTag(GOAL_TARGET_TAG).transform);
    }

}
