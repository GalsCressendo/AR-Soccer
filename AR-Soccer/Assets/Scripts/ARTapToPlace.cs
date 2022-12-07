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

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    bool IsTouching(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!IsTouching(out Vector2 touchPosition))
            return;

        if(arRaycastManager.Raycast(touchPosition,hits, TrackableType.Planes))
        {
            var hitPose = hits[0].pose;

            if(spawnObject == null)
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



}
