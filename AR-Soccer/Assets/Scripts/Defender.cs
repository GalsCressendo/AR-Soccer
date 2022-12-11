using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    const string GAME_MANAGER_TAG = "GameManager";
    const float chaseAttackerSpeed = 1.0f;
    const float returnSpeed = 2.0f;
    const float inactiveDuration = 4.0f;

    bool isActive = true;
    bool isSpawned = false;

    Vector3 initialPosition;

    Transform attacker;
    public GameObject detection;

    [SerializeField] Renderer surfaceRenderer;
    [SerializeField] Renderer boneRenderer;
    Color surfaceActiveColor;
    Color boneActiveColor;

    private void Awake()
    {
        initialPosition = transform.position;

        surfaceActiveColor = surfaceRenderer.material.color;
        boneActiveColor = boneRenderer.material.color;

        isSpawned = true;
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG).GetComponent<GameManager>().gameIsActive)
        {
            if (isSpawned)
            {
                surfaceRenderer.material.color = new Color(surfaceActiveColor.r, surfaceActiveColor.g, surfaceActiveColor.b, 0.5f);
                boneRenderer.material.color = new Color(boneActiveColor.r, boneActiveColor.g, boneActiveColor.b, 0.5f);
            }
            else
            {
                if (attacker != null)
                {
                    var step = chaseAttackerSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, attacker.position, step);

                    if (Vector3.Distance(transform.position, attacker.position) < 0.001f)
                    {
                        attacker.GetComponent<Attacker>().PassBall();
                        attacker = null;
                        isActive = false;
                        detection.SetActive(false);
                    }
                    else if (!attacker.gameObject.activeSelf) //if targeted attacker already reach goal before captured
                    {
                        Invoke("ReturnPosition", inactiveDuration);
                    }
                }

                if (!isActive)
                {
                    surfaceRenderer.material.color = new Color(surfaceActiveColor.r, surfaceActiveColor.g, surfaceActiveColor.b, 0.5f);
                    boneRenderer.material.color = new Color(boneActiveColor.r, boneActiveColor.g, boneActiveColor.b, 0.5f);
                    Invoke("ReturnPosition", inactiveDuration);

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

    public void ReturnPosition()
    {
        var step = returnSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

        if (Vector3.Distance(transform.position, initialPosition) < 0.001f)
        {
            SetActive();
        }
    }

    public void SetActive()
    {
        surfaceRenderer.material.color = surfaceActiveColor;
        boneRenderer.material.color = boneActiveColor;
        isActive = true;
        detection.SetActive(true);
    }

    public void ReactiveAfterSpawn()
    {
        surfaceRenderer.material.color = surfaceActiveColor;
        boneRenderer.material.color = boneActiveColor;
        isSpawned = false;
        detection.SetActive(true);
    }


}
