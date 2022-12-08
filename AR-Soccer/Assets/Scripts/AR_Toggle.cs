using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class AR_Toggle : MonoBehaviour, IPointerClickHandler
{
    Slider slider;
    public Image handleImage;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(slider.value == 0) //off state
        {
            //turn it on
            slider.value = 1;
            handleImage.color = Color.blue;
        }
        else //on state
        {
            //turn it off
            slider.value = 0;
            handleImage.color = Color.red;
        }
    }
}
