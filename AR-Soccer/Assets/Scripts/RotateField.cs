using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateField : MonoBehaviour
{
    public bool isTouched;

    private void Update()
    {
        if (isTouched)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Moved)
                {
                    gameObject.transform.Rotate(new Vector3(0, touch.deltaPosition.x, 0f));
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    isTouched = false;
                }
                
            }
        }
       
    }
}
