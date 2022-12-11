using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    const float ENERGY_PER_SECONDS = 0.5f;

    [SerializeField] Slider inactiveSlider;
    
    [Serializable]
    public struct ActiveSlider
    {
        public int value;
        public GameObject bar;
    }

    public List<ActiveSlider> activeSliders;

    private void Update()
    {
        inactiveSlider.value = Mathf.MoveTowards(inactiveSlider.value, inactiveSlider.value + ENERGY_PER_SECONDS , Time.deltaTime/2);
    }
}
