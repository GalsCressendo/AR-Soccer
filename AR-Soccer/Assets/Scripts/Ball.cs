using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isAttached = false;
    Transform target = null;
    const float speed = 1.5f;

    public void SetAttached()
    {
        isAttached = !isAttached;
    }

    private void Update()
    {
        if (target != null)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            if (Vector3.Distance(transform.position, target.position) < 0.4f)
            {
                transform.SetParent(target.transform, true);
                if(target.tag == "Player")
                {
                    target.GetComponent<PlayerAttack>().haveBall = true;
                    target.GetComponent<PlayerAttack>().highlight.SetActive(true);
                }
                target = null;
            }
        }
    }

    public void PassBallToNearest(Transform target)
    {
        this.target = target;
    }


}
