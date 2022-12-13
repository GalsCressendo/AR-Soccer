using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    SceneLoader sceneLoader = new SceneLoader();

    public void OnPlayButtonClicked()
    {
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
        sceneLoader.SwitchToMainGame();
    }

    public void OnExitButtonClicked()
    {
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.CLICK_SFX);
        Debug.Log("Quit");
        Application.Quit();
    }
}
