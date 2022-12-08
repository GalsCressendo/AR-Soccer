using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class AR_ObjectManager : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject spawnObject;
    ARRaycastManager arRaycastManager;
    ARPlaneManager arPlaneManager;
    Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public AR_Toggle arToggle;

    //PinchToZoom
    Vector2 firstTouch;
    Vector2 secondTouch;
    float distanceCurrent;
    float distancePrevious;
    bool firstPinch = true;

    //Store initial position
    public Vector3 arPosition;
    public Quaternion arRotation;
    public Vector3 arScale;
    

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if (spawnObject == null && gameObject.activeInHierarchy)
        {
            SetPlaneTrackables(true);
        }
        else
        {
            SetPlaneTrackables(false);
        }

        if (Input.touchCount > 0 && spawnObject == null)
        {
            touchPosition = Input.GetTouch(0).position;

            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                spawnObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                arToggle.fieldGameObject = spawnObject;
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
        if (Input.touchCount == 1 && spawnObject != null && Input.GetTouch(0).phase == TouchPhase.Moved && spawnObject != null)
        {
            Touch touch = Input.GetTouch(0);
            spawnObject.transform.Rotate(new Vector3(0, touch.deltaPosition.x, 0f));
        }
    }

    void SetPlaneTrackables(bool state)
    {
        if (state)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(state);
            }

            arPlaneManager.enabled = state;
        }
        else
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }

            arPlaneManager.enabled = false;
        }
    }



}
