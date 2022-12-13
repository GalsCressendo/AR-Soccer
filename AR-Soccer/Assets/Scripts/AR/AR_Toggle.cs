using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AR_Toggle : MonoBehaviour, IPointerClickHandler
{
    Slider slider;
    public Image handleImage;
    public GameObject AR;
    public AR_ObjectManager arObjectManager;
    public GameObject mainCamera;
    Vector3 fieldPosition = new Vector3(0, 0, 1f);
    Quaternion fieldRotation = Quaternion.Euler(new Vector3(90, 0, 0));
    Vector3 fieldScale = Vector3.one;

    [SerializeField] GameManager gameManager;


    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (slider.value == 0) //off state
        {
            //turn it on
            slider.value = 1;
            handleImage.color = Color.blue;
            AR.SetActive(true);
            mainCamera.SetActive(false);

            if (arObjectManager.fieldIsPlaced)
            {
                arObjectManager.fieldGameObject.transform.SetPositionAndRotation(arObjectManager.arPosition, arObjectManager.arRotation);
                arObjectManager.fieldGameObject.transform.localScale = arObjectManager.arScale;
            }
            else
            {
                arObjectManager.fieldGameObject.SetActive(false);
            }

            Debug.Log("[UNITY] offScale:" + arObjectManager.fieldGameObject.transform.localScale);


        }
        else //on state
        {
            //turn it off
            slider.value = 0;
            handleImage.color = Color.red;
            AR.SetActive(false);
            mainCamera.SetActive(true);
            gameManager.gameIsActive = true;

            if(arObjectManager.arPosition == Vector3.zero || arObjectManager.arRotation == Quaternion.Euler(Vector3.zero) || arObjectManager.arScale == Vector3.zero)
            {
                arObjectManager.arPosition = arObjectManager.fieldGameObject.transform.position;
                arObjectManager.arRotation = arObjectManager.fieldGameObject.transform.rotation;
                arObjectManager.arScale = arObjectManager.fieldGameObject.transform.localScale;
            }

            arObjectManager.fieldGameObject.transform.SetPositionAndRotation(fieldPosition, fieldRotation);
            arObjectManager.fieldGameObject.transform.localScale = fieldScale;
            arObjectManager.fieldGameObject.SetActive(true);

            Debug.Log("[UNITY] onScale:" + arObjectManager.fieldGameObject.transform.localScale);

        }

    }
}
