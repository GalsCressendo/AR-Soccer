using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float chaseBallSpeed = 1.5f;
    public GameObject arrow;
    Transform ballTarget;
    public const string BALL_TAG = "Ball";
    public bool haveBall;

    private void Awake()
    {
        ballTarget = GameObject.FindGameObjectWithTag(BALL_TAG).transform;
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

            var step = chaseBallSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, ballTarget.position, step);

            if (Vector3.Distance(transform.position, ballTarget.position) < 0.4f)
            {
                ballTarget.transform.SetParent(gameObject.transform, true);
                haveBall = true;
                ballTarget.GetComponent<Ball>().SetAttached();
                ballTarget = null;
            }
        }
       
    }
}
