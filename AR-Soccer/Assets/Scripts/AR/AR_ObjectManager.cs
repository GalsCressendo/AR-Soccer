using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class AR_ObjectManager : MonoBehaviour
{
    public GameObject fieldGameObject;
    ARRaycastManager arRaycastManager;
    ARPlaneManager arPlaneManager;
    Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public AR_Toggle arToggle;
    public bool fieldIsPlaced;
    [SerializeField] GameManager gameManager;

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
        if (!fieldIsPlaced && gameObject.activeInHierarchy)
        {
            SetPlaneTrackables(true);
            gameManager.GamePaused();
        }

        if (Input.touchCount > 0 && !fieldIsPlaced)
        {
            touchPosition = Input.GetTouch(0).position;

            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                fieldGameObject.transform.position = hitPose.position;
                fieldGameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                fieldGameObject.SetActive(true);
                SetPlaneTrackables(false);
                gameManager.GameActive();
                fieldIsPlaced = true;
            }
        }


        //Pinch to zoom
        if (Input.touchCount == 2 && fieldIsPlaced)
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
                Vector3 scaleValue = fieldGameObject.transform.localScale * (distanceCurrent / distancePrevious);
                fieldGameObject.transform.localScale = scaleValue;
                distancePrevious = distanceCurrent;
            }
        }
        else
        {
            firstPinch = true;
        }

        //Detect touch on field
        if (Input.touchCount == 1 && fieldGameObject != null && Input.GetTouch(0).phase == TouchPhase.Moved && fieldIsPlaced)
        {
            Touch touch = Input.GetTouch(0);
            fieldGameObject.transform.Rotate(new Vector3(0f, touch.deltaPosition.x, 0f));
        }
    }

    void SetPlaneTrackables(bool state)
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(state);
        }

        arPlaneManager.enabled = state;
    }



}
