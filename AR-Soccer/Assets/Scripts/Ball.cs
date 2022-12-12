using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isAttached = false;
    Transform target = null;
    const float PASS_SPEED = 1.5f;
    const float ROTATE_SPEED = 100f;
    const float ROTATE_ANGLE = 80f;
    const string PLAYER_TAG = "Player";

    public void SetAttached()
    {
        isAttached = !isAttached;
    }

    private void Update()
    {
        if (target != null)
        {
            var step = PASS_SPEED * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            if (Vector3.Distance(transform.position, target.position) < 0.4f)
            {
                transform.SetParent(target.transform, true);
                if(target.tag == "Attacker")
                {
                    target.GetComponent<Attacker>().haveBall = true;
                    target.GetComponent<Attacker>().highlight.SetActive(true);
                    target.GetComponent<Attacker>().isReceiving = false;
                }
                target = null;
            }
        }

        if (isAttached)
        {
            RotateBall();
        }

    }

    public void PassBallToNearest(Transform target)
    {
        this.target = target;
    }

    private void RotateBall()
    {
        if(transform.parent.parent.tag == PLAYER_TAG)
        {
            transform.Rotate(new Vector3(ROTATE_ANGLE, 0, 0), ROTATE_SPEED * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(-ROTATE_ANGLE, 0, 0), ROTATE_SPEED * Time.deltaTime);
        }
       
    }
}
