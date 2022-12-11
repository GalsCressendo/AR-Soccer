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
    bool addValue = false;
    int targetValue = 1;
    public int activePoints = 0;

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
            activePoints += 1;
        }
    }

    public void ReduceEnergy(int cost)
    {
        inactiveSlider.value -= cost;
        int targetPoint = activePoints - cost;
        for(int i=activePoints; i > targetPoint;i--)
        {
            activeSliders[i - 1].SetActive(false);

        }

        activePoints -= cost;
        targetValue -= cost;
    }

    public void ResetEnergyBar()
    {
        addValue = false;
        targetValue = 1;
        activePoints = 0;
        inactiveSlider.value = activePoints;
        foreach(GameObject bar in activeSliders)
        {
            bar.SetActive(false);
        }

    }

    public void StartEnergyBar()
    {
        addValue = true;
    }


}
