using System.Collections;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    const string BALL_TAG = "Ball";
    const string GAME_MANAGER_TAG = "GameManager";
    const string PLAYER_TAG = "Player";
    const float ROTATION_SPEED = 200;
    const float CHASE_BALL_SPEED = 1.5f;
    const float CARRY_BALL_SPEED = 0.75f;
    const float REACTIVE_TIME = 2.5f;

    public UnitAttributes attributes;

    Transform ballTarget;
    Transform goalTarget;
    Quaternion initialRotation;

    [SerializeField] Renderer surfaceRenderer;
    Material activeMaterial;

    public GameObject highlight;
    [SerializeField] GameObject indicator;

    public bool isActive;
    public bool haveBall;
    public bool isCaptured;
    bool isSpawned;
    public bool isReceiving;

    public UnitContainer unitContainer;

    [SerializeField] private Animator animator;
    const string RUN_ANIM_PARAM = "isRunning";
    const string CAPTURED_ANIM_PARAM = "isCaptured";

    [SerializeField] private GameObject confettiEffect;

    private void Awake()
    {
        goalTarget = GameObject.FindGameObjectWithTag(attributes.GOAL_TAG).transform;
        activeMaterial = surfaceRenderer.material;
        isSpawned = true;
        initialRotation = transform.rotation;
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

                    MoveTowardsTarget(ballTarget, CHASE_BALL_SPEED);

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

                    if (gameObject.transform.parent.tag == PLAYER_TAG)
                    {
                        transform.position += new Vector3(0, 0, CARRY_BALL_SPEED) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position +=  new Vector3(0, 0, -CARRY_BALL_SPEED) * Time.deltaTime;
                    }

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, ROTATION_SPEED * Time.deltaTime);
                    PlayRunningAnim();

                }

                //if receiving a ball
                if (isReceiving)
                {
                    RotateTowardsTarget(GameObject.FindGameObjectWithTag(BALL_TAG).transform.position);
                    StartCoroutine(ReceivingBall());
                }

                //If the attacker carry a ball
                if (haveBall)
                {
                    isActive = true;
                    highlight.SetActive(true);
                    MoveTowardsTarget(goalTarget, CARRY_BALL_SPEED);
                    SetActiveColor();
                }

                //if attacker is captured
                if (isCaptured)
                {
                    Invoke("ReactiveAfterCaptured", REACTIVE_TIME);
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
        if (other.tag == attributes.FENCE_TAG)
        {
            Destroy(gameObject);
        }
        else if (other.tag == attributes.GOAL_TAG)
        {
            if (haveBall)
            {
                FindObjectOfType<AudioManager>().PlayAudio(AudioManager.GOAL_SFX);
                GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().DeclareWinAttacker();
                CreateConfettiEffect(GameObject.FindGameObjectWithTag(attributes.GOAL_TAG).transform);
            }

            Destroy(gameObject);

        }
    }

    public void PassBall()
    {
        if (haveBall)
        {
            isActive = false;
            PlayCapturedAnim();
            isCaptured = true;
            highlight.SetActive(false);
            SetInactiveColor();

            var nearestDistance = float.MaxValue;
            Transform nearestTransform = null;
            foreach (Transform t in unitContainer.GetAllUnitTransform(gameObject.transform))
            {
                if (t.GetComponent<Attacker>().isActive)
                {
                    if (Vector3.Distance(transform.position, t.position) < nearestDistance)
                    {
                        nearestDistance = Vector3.Distance(transform.position, t.position);
                        nearestTransform = t;
                    }
                }

            }

            ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).GetComponent<Transform>();

            //if there is any other unit to pass the ball
            if (nearestTransform != null)
            {
                nearestTransform.GetComponent<Attacker>().isReceiving = true;
                ballTarget.GetComponent<Ball>().PassBallToNearest(nearestTransform);
                FindObjectOfType<AudioManager>().PlayAudio(AudioManager.BALL_KICK_SFX);
            }
            else //if there is no one to pass the ball
            {
                GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().DeclareDraw();
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

        if (GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().gameIsActive)
        {
            indicator.SetActive(true);
            animator.SetBool(RUN_ANIM_PARAM, true);
            animator.ResetTrigger(CAPTURED_ANIM_PARAM);
        }

    }

    private void PlayCapturedAnim()
    {
        indicator.SetActive(false);
        animator.SetTrigger(CAPTURED_ANIM_PARAM);
    }

    private IEnumerator ReceivingBall()
    {
        RunToIdle();
        yield return new WaitUntil(() => isReceiving == false);
    }

    private void RunToIdle()
    {
        animator.SetBool(RUN_ANIM_PARAM, false);
        indicator.SetActive(false);
    }

    private void CreateConfettiEffect(Transform targetTransform)
    {
        GameObject confetti = Instantiate(confettiEffect, confettiEffect.transform.position, confettiEffect.transform.rotation);
        confetti.transform.localScale = FindObjectOfType<GameManager>().GetCurrentFieldRatioScale(confetti.transform.localScale);
        confetti.GetComponent<Confetti>().GetConffeti(targetTransform);
    }


}
