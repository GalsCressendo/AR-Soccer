using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float matchTime = 140;
    float timeRemaining;
    public bool isTicking;
    [SerializeField] TextMeshProUGUI countdownText;
    const string MAIN_SCENE_NAME = "Main";
    const string PENALTY_SCENE_NAME = "PenaltyGame";
    const string GAME_MANAGER_TAG = "GameManager";

    private void Start()
    {
        timeRemaining = matchTime;
    }

    private void Update()
    {
        if (isTicking)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                if(timeRemaining <= 10)
                {
                    countdownText.color = Color.red;
                }
            }
            else
            {
                timeRemaining = 0;
                isTicking = false;

                Scene scene = SceneManager.GetActiveScene();
                GameObject gameManager = GameObject.FindGameObjectWithTag(GAME_MANAGER_TAG);
                if (scene.name == MAIN_SCENE_NAME)
                {
                    gameManager.GetComponent<GameManager>().DeclareWinDefender();
                }
                else if(scene.name == PENALTY_SCENE_NAME)
                {
                    gameManager.GetComponent<MazeGameManager>().DeclareEnemyWinner();
                }
            }
        }
        
    }

    void DisplayTime(float time)
    {
        time += 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTime()
    {
        isTicking = true;
        timeRemaining = matchTime;
        countdownText.color = Color.white;
    }
}
