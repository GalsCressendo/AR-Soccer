using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    SceneLoader sceneLoader = new SceneLoader();

    public void OnPlayButtonClicked()
    {
        sceneLoader.SwitchToMainGame();
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
