using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltyButtonManager : MonoBehaviour
{
    SceneLoader sceneLoader = new SceneLoader();
    [SerializeField] MazeGameManager mazeGameManager;
    [SerializeField] GameObject popUpMenu;

    public void OnPauseButtonClicked()
    {
        if (mazeGameManager.gameIsActive)
        {
            FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
            mazeGameManager.PauseGame();
            popUpMenu.SetActive(true);
        }

    }

    public void OnExitButtonClick()
    {
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
        mazeGameManager.gameIsActive = true;
        popUpMenu.SetActive(false);
    }

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
}
