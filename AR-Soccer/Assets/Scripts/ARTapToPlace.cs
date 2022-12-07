using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    private GameObject spawnObject;
    ARRaycastManager arRaycastManager;
    ARPlaneManager arPlaneManager;
    Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //PinchToZoom
    Vector2 first_touch;
    Vector2 second_touch;
    float distance_current;
    float distance_previous;
    bool first_pinch = true;

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {

            touchPosition = Input.GetTouch(0).position;

            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
            {
                var hitPose = hits[0].pose;

                if (spawnObject == null)
                {
                    spawnObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                    foreach (var plane in arPlaneManager.trackables)
                    {
                        plane.gameObject.SetActive(false);
                    }

                    arPlaneManager.enabled = false;

                }
            }
        }

       

        if(Input.touchCount>1 && spawnObject != null)
        {
            first_touch = Input.GetTouch(0).position;
            second_touch = Input.GetTouch(1).position;

            distance_current = second_touch.magnitude - first_touch.magnitude;
            if (first_pinch)
            {
                distance_previous = distance_current;
                first_pinch = false;
            }

            if (distance_current != distance_previous)
            {
                Vector3 scale_value = spawnObject.transform.localScale * (distance_current / distance_previous);
                spawnObject.transform.localScale = scale_value;
                distance_previous = distance_current;
            }
        }
        else
        {
            first_pinch = true;
        }
    }



}
