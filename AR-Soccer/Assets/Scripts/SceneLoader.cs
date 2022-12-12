using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    const string MAIN_SCENE_NAME = "Main";
    const string PENALTY_SCENE_NAME = "PenaltyGame";

    public void SwitchToMainGame()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    public void SwitchToPenaltyGame()
    {
        SceneManager.LoadScene(PENALTY_SCENE_NAME);
    }
}
