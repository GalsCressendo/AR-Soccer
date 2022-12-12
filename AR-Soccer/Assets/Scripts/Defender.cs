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

    Vector3 initialPosition;
    Quaternion initialRotation;

    Transform attacker;
    public GameObject detection;

    [SerializeField] Renderer surfaceRenderer;
    Material activeMaterial;

    public UnitAttributes attributes;

    [SerializeField] private Animator animator;
    const string RUN_ANIM_PARAM = "isRunning";

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
                        PlayRunningAnim(true);
                    }


                    if (Vector3.Distance(transform.position, attacker.position) < 0.001f)
                    {
                        attacker.GetComponent<Attacker>().PassBall();
                        isActive = false;
                        detection.SetActive(false);
                        PlayRunningAnim(false);
                        StartCoroutine(ReturnPosition());
                    }
                    else if (!attacker.gameObject.activeSelf) //if targeted attacker already reach goal before captured
                    {
                        StartCoroutine(ReturnPosition());
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
        SetInactiveColor();
        yield return new WaitForSeconds(INACTIVE_DURATION);

        var step = RETURN_SPEED * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
        RotateTowardsTarget(initialPosition);
        transform.rotation = initialRotation;

        PlayRunningAnim(true);

        if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
        {
            SetWaitingState();
        }

    }

    public void SetWaitingState()
    {
        SetInactiveColor();
        isActive = true;
        detection.SetActive(true);
        PlayRunningAnim(false);
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

    private void PlayRunningAnim(bool isRunning)
    {
        animator.SetBool(RUN_ANIM_PARAM, isRunning);
    }

}
