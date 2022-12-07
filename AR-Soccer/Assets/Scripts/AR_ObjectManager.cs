using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class AR_ObjectManager : MonoBehaviour
{
    public GameObject objectToPlace;
    private GameObject spawnObject;
    ARRaycastManager arRaycastManager;
    ARPlaneManager arPlaneManager;
    Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public Camera arCamera;

    //PinchToZoom
    Vector2 firstTouch;
    Vector2 secondTouch;
    float distanceCurrent;
    float distancePrevious;
    bool firstPinch = true;

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && spawnObject == null)
        {
            touchPosition = Input.GetTouch(0).position;

            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
            {
                var hitPose = hits[0].pose;
                spawnObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                foreach (var plane in arPlaneManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }

                arPlaneManager.enabled = false;

            }
        }


        //Pinch to zoom
        if (Input.touchCount == 2 && spawnObject != null)
        {
            firstTouch = Input.GetTouch(0).position;
            secondTouch = Input.GetTouch(1).position;

            distanceCurrent = secondTouch.magnitude - firstTouch.magnitude;
            if (firstPinch)
            {
                distancePrevious = distanceCurrent;
                firstPinch = false;
            }

            if (distanceCurrent != distancePrevious)
            {
                Vector3 scaleValue = spawnObject.transform.localScale * (distanceCurrent / distancePrevious);
                spawnObject.transform.localScale = scaleValue;
                distancePrevious = distanceCurrent;
            }
        }
        else
        {
            firstPinch = true;
        }

        //Detect touch on field
        if(Input.touchCount == 1 && spawnObject!=null && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "Field")
                {
                    var rotateField = hit.collider.GetComponent<RotateField>();
                    rotateField.isTouched = !rotateField.isTouched;
                }
            }
        }
    }



}
