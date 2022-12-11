using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    const float ENERGY_PER_SECONDS = 0.5f;

    [SerializeField] Slider inactiveSlider;

    public List<GameObject> activeSliders;
    bool addValue = true;
    int targetValue = 1;

    private void Update()
    {
        if (addValue)
        {
            inactiveSlider.value = Mathf.MoveTowards(inactiveSlider.value, targetValue, ENERGY_PER_SECONDS * Time.deltaTime);
        }
        
    }

    public void OnInactiveValueChanged()
    {
        if(inactiveSlider.value == targetValue)
        {
            activeSliders[targetValue - 1].SetActive(true);
            targetValue += 1;
        }
    }


}
