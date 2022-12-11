using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    int playerScore = 0;
    int enemyScore = 0;

    [SerializeField]TextMeshProUGUI playerScoreText;
    [SerializeField]TextMeshProUGUI enemyScoreText;

    public void AddPlayerScore()
    {
        playerScore += 1;
        playerScoreText.text = string.Format("Score:{0}", playerScore);
    }

    public void AddEnemyScore()
    {
        enemyScore += 1;
        enemyScoreText.text = string.Format("Score:{0}",enemyScore);
    }

}
