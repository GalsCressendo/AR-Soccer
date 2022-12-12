using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject menuPopUp;
    [SerializeField] GameManager gameManager;
    [SerializeField] Timer timer;
    [SerializeField] EnergyBar energyBarRed;
    [SerializeField] EnergyBar energyBarBlue;

    public void OnPauseButtonClick()
    {
        timer.isTicking = false;
        menuPopUp.SetActive(true);
        gameManager.gameIsActive = false;
        energyBarRed.StopEnergyBar();
        energyBarBlue.StopEnergyBar();
    }
}
