using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    const string MAIN_SCENE_NAME = "Main";
    const string PENALTY_SCENE_NAME = "PenaltyGame";
    const string START_MENU_SCENE_NAME = "StartMenu";

    public void SwitchToMainGame()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    public void SwitchToPenaltyGame()
    {
        SceneManager.LoadScene(PENALTY_SCENE_NAME);
    }

    public void SwitchToStartMenuGame()
    {
        SceneManager.LoadScene(START_MENU_SCENE_NAME);
    }
}
