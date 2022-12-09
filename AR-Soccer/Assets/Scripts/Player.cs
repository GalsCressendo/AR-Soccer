using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const string BALL_TAG = "Ball";
    public const string GOAL_TAG = "Goal_Red";

    float chaseBallSpeed = 1.5f;
    float carryBallSpeed = 0.75f;
    public GameObject arrow;
    Transform ballTarget;
    Transform goalTarget;
    public bool haveBall;

    private void Awake()
    {
        ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
        goalTarget = GameObject.FindGameObjectWithTag(GOAL_TAG).transform;
    }

    private void Update()
    {

        if (ballTarget != null)
        {
            if (ballTarget.GetComponent<Ball>().isAttached)
            {
                arrow.SetActive(false);
                ballTarget = null;
                return;
            }

            arrow.SetActive(true);

            MoveTowardsTarget(ballTarget, chaseBallSpeed);

            if (Vector3.Distance(transform.position, ballTarget.position) < 0.4f)
            {
                ballTarget.transform.SetParent(gameObject.transform, true);
                haveBall = true;
                ballTarget.GetComponent<Ball>().SetAttached();
                ballTarget = null;
            }
        }

        if (haveBall)
        {
            MoveTowardsTarget(goalTarget, carryBallSpeed);
        }
       
    }

    void MoveTowardsTarget(Transform target, float speed)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        //TODO: ROTATE PLAYERS
        //transform.Rotate(new Vector3(0f, target.position.y, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GOAL_TAG)
        {
            Destroy(gameObject);
            //TO DO: ADD SCORE
        }
    }
}
