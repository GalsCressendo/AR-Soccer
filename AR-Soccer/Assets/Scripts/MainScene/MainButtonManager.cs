using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonManager : MonoBehaviour
{
    SceneLoader sceneLoader = new SceneLoader();
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject popUpMenu;

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
        gameManager.GameActive();
        popUpMenu.SetActive(false);
    }

    public void OnPauseButtonClick()
    {
        if (!gameManager.mainGameOver)
        {
            FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
            popUpMenu.SetActive(true);
            gameManager.GamePaused();
        }

    }
}
