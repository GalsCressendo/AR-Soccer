using System.Collections;
using UnityEngine;

public class Defender : MonoBehaviour
{
    const string GAME_MANAGER_TAG = "GameManager";
    const float CHASE_ATTACKER_SPEED = 1.0f;
    const float RETURN_SPEED = 2.0f;
    const float INACTIVE_DURATION = 4.0f;
    const float ROTATION_SPEED = 200;

    bool isActive = true;
    bool isSpawned = false;
    bool haveReturned = true;

    Vector3 initialPosition;
    Quaternion initialRotation;

    Transform attacker;
    public GameObject detection;
    [SerializeField] GameObject indicator;

    [SerializeField] Renderer surfaceRenderer;
    Material activeMaterial;

    public UnitAttributes attributes;

    [SerializeField] private Animator animator;
    const string RUN_ANIM_PARAM = "isRunning";
    const string ATTACK_ANIM_PARAM = "isAttacking";

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

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
                if (attacker != null)
                {
                    if (isActive)
                    {
                        var step = CHASE_ATTACKER_SPEED * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, attacker.position, step);
                        RotateTowardsTarget(attacker.position);
                        SetActiveColor();
                        PlayRunningAnim();

                        if (Vector3.Distance(transform.position, attacker.position) < 0.5f)
                        {
                            attacker.GetComponent<Attacker>().PassBall();
                            PlayAttackAnim();
                            FindObjectOfType<AudioManager>().PlayAudio(AudioManager.PUNCH_SFX);
                            SetReturnState();

                        }
                        //if attacker get inactive when being chased, ex: when another defender already caught it
                        else if (!attacker.GetComponent<Attacker>().isActive)
                        {
                            SetReturnState();
                        }
                    }

                }

                if (!haveReturned)
                {
                    var step = RETURN_SPEED * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
                    RotateTowardsTarget(initialPosition);


                    if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
                    {
                        SetWaitingState();
                    }
                }

            }
        }

    }

    public void ChaseAttacker(Transform attacker)
    {
        if (isActive)
        {
            if (attacker.GetComponent<Attacker>().haveBall)
            {
                this.attacker = attacker;
            }
        }

    }

    void RotateTowardsTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, ROTATION_SPEED * Time.deltaTime);
    }

    private IEnumerator ReturnPosition()
    {
        yield return new WaitForSeconds(INACTIVE_DURATION);
        haveReturned = false;
        PlayRunningAnim();
    }

    public void SetWaitingState()
    {
        haveReturned = true;
        SetInactiveColor();
        isActive = true;
        detection.SetActive(true);
        attacker = null;
        RunToIdle();
        transform.rotation = initialRotation;
    }

    public void SetReturnState()
    {
        isActive = false;
        detection.SetActive(false);
        SetInactiveColor();
        StartCoroutine(ReturnPosition());
        attacker = null;
    }

    public void ReactiveAfterSpawn()
    {
        SetInactiveColor();
        isSpawned = false;
        detection.SetActive(true);
    }

    private void SetActiveColor()
    {
        surfaceRenderer.material = activeMaterial;
    }

    private void SetInactiveColor()
    {
        surfaceRenderer.material = attributes.inactiveMaterial;
    }

    private void PlayRunningAnim()
    {
        if (GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().gameIsActive)
        {
            indicator.SetActive(true);
            animator.SetBool(RUN_ANIM_PARAM, true);
            animator.ResetTrigger(ATTACK_ANIM_PARAM);
        }

    }

    private void PlayAttackAnim()
    {

        indicator.SetActive(false);
        animator.SetTrigger(ATTACK_ANIM_PARAM);
        animator.SetBool(RUN_ANIM_PARAM, false);

    }

    private void RunToIdle()
    {
        animator.SetBool(RUN_ANIM_PARAM, false);
        indicator.SetActive(false);
    }

}
