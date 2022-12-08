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

    public GameObject fieldGameObject;
    Vector3 fieldPosition = new Vector3(0, 0, 10f);
    Quaternion fieldRotation = Quaternion.Euler(new Vector3(-100, 0, 0));
    Vector3 fieldScale = new Vector3(0.45f, 0, 0.9f);


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
            

            if (fieldGameObject != null)
            {
                fieldGameObject.transform.SetPositionAndRotation(arObjectManager.arPosition, arObjectManager.arRotation);
                fieldGameObject.transform.localScale = arObjectManager.arScale;
            }


        }
        else //on state
        {
            //turn it off
            slider.value = 0;
            handleImage.color = Color.red;
            AR.SetActive(false);
            mainCamera.SetActive(true);

            
            if(arObjectManager.arPosition == Vector3.zero || arObjectManager.arRotation == Quaternion.Euler(Vector3.zero) || arObjectManager.arScale == Vector3.zero)
            {
                arObjectManager.arPosition = arObjectManager.spawnObject.transform.position;
                arObjectManager.arRotation = arObjectManager.spawnObject.transform.rotation;
                arObjectManager.arScale = arObjectManager.spawnObject.transform.localScale;
            }

            fieldGameObject.transform.SetPositionAndRotation(fieldPosition, fieldRotation);
            fieldGameObject.transform.localScale = fieldScale;

        }

    }
}
