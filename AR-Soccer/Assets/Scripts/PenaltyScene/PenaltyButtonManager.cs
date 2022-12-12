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
        mazeGameManager.PauseGame();
        popUpMenu.SetActive(true);
    }

    public void OnExitButtonClick()
    {
        mazeGameManager.gameIsActive = true;
        popUpMenu.SetActive(false);
    }

    public void OnReplayButtonClick()
    {
        sceneLoader.SwitchToMainGame();
    }

    public void OnMenuButtonClick()
    {
        sceneLoader.SwitchToStartMenuGame();
    }
}
