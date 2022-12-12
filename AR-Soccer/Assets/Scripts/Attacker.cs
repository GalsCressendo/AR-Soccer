using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    const string BALL_TAG = "Ball";
    const string GAME_MANAGER_TAG = "GameManager";
    const string PLAYER_TAG = "Player";
    const float ROTATION_SPEED = 200;

    public UnitAttributes attributes;

    const float chaseBallSpeed = 1.5f;
    const float carryBallSpeed = 0.75f;
    const float reactiveTime = 2.5f;

    Transform ballTarget;
    Transform goalTarget;

    [SerializeField] Renderer surfaceRenderer;
    Material activeMaterial;

    public GameObject highlight;

    public bool haveBall;
    public bool isCaptured;
    bool isSpawned = false;
    public bool isReceiving = false;

    public UnitContainer unitContainer;

    [SerializeField] private Animator animator;
    const string RUN_ANIM_PARAM = "isRunning";
    const string CAPTURED_ANIM_PARAM = "isCaptured";

    private void Awake()
    {
        goalTarget = GameObject.FindGameObjectWithTag(attributes.GOAL_TAG).transform;

        activeMaterial = surfaceRenderer.material;

        isSpawned = true;
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().gameIsActive)
        {
            if (isSpawned)
            {
                SetInactiveColor();
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

                    if (Vector3.Distance(transform.position, ballTarget.position) < 0.3f)
                    {
                        ballTarget.transform.SetParent(gameObject.transform, true);
                        haveBall = true;
                        ballTarget.GetComponent<Ball>().SetAttached();
                        ballTarget = null;
                    }
                }

                //if not carrying a ball
                if (ballTarget == null && !haveBall && !isCaptured & !isReceiving)
                {
                    if (GameObject.FindGameObjectWithTag(BALL_TAG) != null)
                    {
                        if (!GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Ball>().isAttached)
                        {
                            ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
                            return;
                        }

                    }

                    if(gameObject.transform.parent.tag == PLAYER_TAG)
                    {
                        transform.position += new Vector3(0, 0, carryBallSpeed) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += new Vector3(0, 0, -carryBallSpeed) * Time.deltaTime;
                    }

                    PlayRunningAnim();

                }

                //if receiving a ball
                if (isReceiving)
                {
                    StartCoroutine(ReceivingBall());
                }

                //If the attacker carry a ball
                if (haveBall)
                {
                    highlight.SetActive(true);
                    MoveTowardsTarget(goalTarget, carryBallSpeed);
                    SetActiveColor();
                }

                //if attacker is captured
                if (isCaptured)
                {
                    Invoke("ReactiveAfterCaptured", reactiveTime);
                }

            }
        }
       
    }

    void MoveTowardsTarget(Transform target, float speed)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        if (target.position != Vector3.zero)
        {
            RotateTowardsTarget(target.position);
        }

        PlayRunningAnim();
    }

    void RotateTowardsTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, ROTATION_SPEED * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == attributes.FENCE_TAG)
        {
            Destroy(gameObject);
        }
        else if(other.tag == attributes.GOAL_TAG)
        {
            if (haveBall)
            {
                GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().DeclareWinAttacker();
            }
            Destroy(gameObject);
        }
    }

    public void PassBall()
    {
        if (haveBall)
        {
            animator.SetTrigger(CAPTURED_ANIM_PARAM);
            isCaptured = true;
            highlight.SetActive(false);
            SetInactiveColor();

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

            ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Transform>();

            //if there is any other unit to pass the ball
            if (nearestTransform != null)
            {
                nearestTransform.GetComponent<Attacker>().isReceiving = true;
                ballTarget.GetComponent<Ball>().PassBallToNearest(nearestTransform);
            }
            else //if there is no one to pass the ball
            {
                GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().DeclareWinDefender();
                Destroy(ballTarget.gameObject);
            }

            ballTarget = null;
            haveBall = false;
        }

    }

    private void ReactiveAfterCaptured()
    {
        SetActiveColor();
        isCaptured = false;
    }

    public void ReactiveAfterSpawn()
    {
        SetActiveColor();
        isSpawned = false;
    }

    private void SetActiveColor()
    {
        surfaceRenderer.material = activeMaterial;
    }

    private void SetInactiveColor()
    {
        surfaceRenderer.material = attributes.inactiveMaterial;
    }

    private void OnDestroy()
    {
        unitContainer.GetComponent<UnitContainer>().units.Remove(gameObject);
    }

    private void PlayRunningAnim()
    {
        animator.SetTrigger(RUN_ANIM_PARAM);
    }

    private IEnumerator ReceivingBall()
    {
        RunToIdle();
        yield return new WaitUntil(() => isReceiving == false);
    }

    private void RunToIdle()
    {
        animator.SetBool(RUN_ANIM_PARAM, false);
    }


}
