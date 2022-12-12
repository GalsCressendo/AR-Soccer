using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    SceneLoader sceneLoader = new SceneLoader();
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject popUpMenu;
    [SerializeField] Timer timer;
    [SerializeField] EnergyBar energyBarRed;
    [SerializeField] EnergyBar energyBarBlue;

    public void OnReplayButtonClick()
    {
        sceneLoader.SwitchToMainGame();
    }

    public void OnMenuButtonClick()
    {
        sceneLoader.SwitchToStartMenuGame();
    }

    public void OnExitButtonClick()
    {
        energyBarRed.StartEnergyBar();
        energyBarBlue.StartEnergyBar();
        timer.isTicking = true;
        gameManager.gameIsActive = true;
        popUpMenu.SetActive(false);
    }
    
}
