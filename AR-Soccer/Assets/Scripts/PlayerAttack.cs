using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    const string BALL_TAG = "Ball";
    const string GOAL_TAG = "Goal_Red";
    const string FENCE_TAG = "Fence_Red";
    const string DETECTION_TAG = "Detection";

    const float chaseBallSpeed = 1.5f;
    const float carryBallSpeed = 0.75f;
    const float reactiveTime = 2.5f;

    Transform ballTarget;
    Transform goalTarget;

    Material material;
    Color activeColor;

    public GameObject highlight;

    public bool haveBall;
    public bool isCaptured;

    PlayerContainer playerContainer;

    private void Awake()
    {
        ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
        goalTarget = GameObject.FindGameObjectWithTag(GOAL_TAG).transform;

        playerContainer = transform.parent.GetComponent<PlayerContainer>();

        material = GetComponent<MeshRenderer>().material;
        activeColor = material.color;
    }

    private void Update()
    {
        if (ballTarget != null && !isCaptured)
        {
            if (ballTarget.GetComponent<Ball>().isAttached)
            {
                ballTarget = null;
                return;
            }

            MoveTowardsTarget(ballTarget, chaseBallSpeed);

            if (Vector3.Distance(transform.position, ballTarget.position) < 0.4f)
            {
                ballTarget.transform.SetParent(gameObject.transform, true);
                haveBall = true;
                ballTarget.GetComponent<Ball>().SetAttached();
                ballTarget = null;
            }
        }

        //if inactive
        if (ballTarget == null && !haveBall  && !isCaptured)
        {
            transform.position += new Vector3(0, 0, carryBallSpeed) * Time.deltaTime;
        }


        //If the player carry a ball
        if (haveBall)
        {
            highlight.SetActive(true);
            MoveTowardsTarget(goalTarget, carryBallSpeed);
            material.color = activeColor;
        }

        //if player is captured
        if (isCaptured)
        {
            Invoke("ReactiveAfterCaptured", reactiveTime);
        }
       
    }

    void MoveTowardsTarget(Transform target, float speed)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        //TODO: ROTATE PLAYERS
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case GOAL_TAG:
                Destroy(gameObject);
                //ADD SCORE
                break;
            case FENCE_TAG:
                Destroy(gameObject);
                break;

        }
    }

    public void PassBall()
    {
        if (haveBall)
        {
            isCaptured = true;
            highlight.SetActive(false);
            haveBall = false;
            material.color = new Color(activeColor.r, activeColor.g, activeColor.b, 0.5f);

            var nearestDistance = float.MaxValue;
            Transform nearestTransform = null;
            foreach(Transform t in playerContainer.GetAllPlayersTransform(gameObject.transform))
            {
                if(Vector3.Distance(transform.position, t.position) < nearestDistance)
                {
                    nearestDistance = Vector3.Distance(transform.position, t.position);
                    nearestTransform = t;
                }
            }

            ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Transform>();
            ballTarget.GetComponent<Ball>().PassBallToNearest(nearestTransform);

            ballTarget = null;
        }

    }

    private void ReactiveAfterCaptured()
    {
        material.color = activeColor;
        isCaptured = false;
    }


}
