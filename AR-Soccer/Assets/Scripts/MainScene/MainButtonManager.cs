using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonManager : MonoBehaviour
{
    SceneLoader sceneLoader = new SceneLoader();
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject popUpMenu;
    [SerializeField] Timer timer;
    [SerializeField] EnergyBar energyBarRed;
    [SerializeField] EnergyBar energyBarBlue;

    public void OnReplayButtonClick()
    {
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
        sceneLoader.SwitchToMainGame();
    }

    public void OnMenuButtonClick()
    {
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
        sceneLoader.SwitchToStartMenuGame();
    }

    public void OnExitButtonClick()
    {
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
        energyBarRed.StartEnergyBar();
        energyBarBlue.StartEnergyBar();
        timer.isTicking = true;
        gameManager.gameIsActive = true;
        popUpMenu.SetActive(false);
    }

    public void OnPauseButtonClick()
    {
        if (!gameManager.mainGameOver)
        {
            FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
            timer.isTicking = false;
            popUpMenu.SetActive(true);
            gameManager.gameIsActive = false;
            energyBarRed.StopEnergyBar();
            energyBarBlue.StopEnergyBar();
        }

    }
}
