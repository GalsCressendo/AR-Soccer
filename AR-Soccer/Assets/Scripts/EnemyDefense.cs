using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefense : MonoBehaviour
{
    const string BALL_TAG = "Ball";

    const float chaseAttackerSpeed = 1.0f;
    const float returnSpeed = 2.0f;

    bool isActive = true;

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
    }

    private void Update()
    {
        if (attacker!=null)
        {
            var step = chaseAttackerSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, attacker.position, step);

            if (Vector3.Distance(transform.position, attacker.position) < 0.001f)
            {
                attacker.GetComponent<PlayerAttack>().PassBall();
                attacker = null;
                isActive = false;
                detection.SetActive(false);
            }
        }

        if (!isActive)
        {
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0.5f);

            var step = returnSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

            if (Vector3.Distance(transform.position, initialPosition) < 0.001f)
            {
                SetActive();
            }
        }
    }

    public void ChaseAttacker(Transform attacker)
    {
        if (isActive)
        {
            if (attacker.GetComponent<PlayerAttack>().haveBall)
            {
                this.attacker = attacker;
            }
        }
        
    }

    public void SetActive()
    {
        material.color = initialColor;
        isActive = true;
        detection.SetActive(true);
    }


}
