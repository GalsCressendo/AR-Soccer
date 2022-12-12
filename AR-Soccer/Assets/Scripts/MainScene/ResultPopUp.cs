using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;

    public void SetPlayerWinner()
    {
        string result = "PLAYER WIN!!";
        resultText.text = result;
        resultText.color = Color.blue;
    }

    public void SetEnemyWinner()
    {
        string result = "ENEMY WIN!!";
        resultText.text = result;
        resultText.color = Color.red;
    }

    public void SetResultDraw()
    {
        string result = "DRAW";
        resultText.text = result;
        resultText.color = Color.black;
    }
}
