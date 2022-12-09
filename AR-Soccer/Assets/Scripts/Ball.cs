using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isAttached = false;

    public void SetAttached()
    {
        isAttached = !isAttached;
    }


}
