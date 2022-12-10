using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    const string BALL_TAG = "Ball";
    public UnitAttributes attributes;

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
    bool isSpawned = false;

    public UnitContainer unitContainer;

    private void Awake()
    {
        goalTarget = GameObject.FindGameObjectWithTag(attributes.GOAL_TAG).transform;

        material = GetComponent<MeshRenderer>().material;
        activeColor = material.color;

        isSpawned = true;
    }

    private void Update()
    {
        if (isSpawned)
        {
            material.color = new Color(activeColor.r, activeColor.g, activeColor.b, 0.5f);
        }
        else
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

            //if not carrying a ball
            if (ballTarget == null && !haveBall && !isCaptured)
            {
                if (GameObject.FindGameObjectWithTag(BALL_TAG) != null)
                {
                    if (!GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Ball>().isAttached)
                    {
                        ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
                        return;
                    }
                   
                }

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
       
    }

    void MoveTowardsTarget(Transform target, float speed)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == attributes.GOAL_TAG)
        {
            Destroy(gameObject);
            //ADD SCORE
        }
        else if(other.tag == attributes.FENCE_TAG)
        {
            Destroy(gameObject);
        }
    }

    public void PassBall()
    {
        if (haveBall)
        {
            isCaptured = true;
            highlight.SetActive(false);
            material.color = new Color(activeColor.r, activeColor.g, activeColor.b, 0.5f);

            var nearestDistance = float.MaxValue;
            Transform nearestTransform = null;
            foreach(Transform t in unitContainer.GetAllUnitTransform(gameObject.transform))
            {
                if(Vector3.Distance(transform.position, t.position) < nearestDistance)
                {
                    nearestDistance = Vector3.Distance(transform.position, t.position);
                    nearestTransform = t;
                }
            }

            //if there is any other unit to pass the ball
            if (nearestTransform != null)
            {
                ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Transform>();
                ballTarget.GetComponent<Ball>().PassBallToNearest(nearestTransform);
            }


            ballTarget = null;
            haveBall = false;
        }

    }

    private void ReactiveAfterCaptured()
    {
        material.color = activeColor;
        isCaptured = false;
    }

    public void ReactiveAfterSpawn()
    {
        material.color = activeColor;
        isSpawned = false;
    }


}
