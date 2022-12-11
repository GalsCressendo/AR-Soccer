using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    const float energyPerSeconds = 0.5f;

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

    }
}
