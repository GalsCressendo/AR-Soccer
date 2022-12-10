using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{

    const float chaseAttackerSpeed = 1.0f;
    const float returnSpeed = 2.0f;
    const float inactiveDuration = 4.0f;

    bool isActive = true;
    bool isSpawned = false;

    Vector3 initialPosition;

    Transform attacker;
    public GameObject detection;

    Material material;
    Color initialColor;

    private void Awake()
    {
        initialPosition = transform.position;
        material = gameObject.GetComponent<MeshRenderer>().material;
        initialColor = material.color;
        isSpawned = true;
    }

    private void Update()
    {
        if (isSpawned)
        {
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0.5f);
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
            }

            if (!isActive)
            {
                material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0.5f);
                Invoke("ReturnPosition", inactiveDuration);
                
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
        material.color = initialColor;
        isActive = true;
        detection.SetActive(true);
    }

    public void ReactiveAfterSpawn()
    {
        material.color = initialColor;
        isSpawned = false;
        detection.SetActive(true);
    }


}
