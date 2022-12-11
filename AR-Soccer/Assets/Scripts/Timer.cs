using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    float matchTime = 10;
    float timeRemaining;
    public bool isTicking;
    public GameManager gameManager;
    [SerializeField] TextMeshProUGUI countdownText;

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
                gameManager.DeclareWinDefender();
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
