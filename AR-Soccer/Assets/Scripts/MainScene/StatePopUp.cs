using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePopUp : MonoBehaviour
{
    [SerializeField] public GameObject playerAttackPopUp;
    [SerializeField] public GameObject enemyAttackPopUp;

    public void ShowPlayerAttack()
    {
        playerAttackPopUp.SetActive(true);
        enemyAttackPopUp.SetActive(false);
    }

    public void ShowEnemyAttack()
    {
        playerAttackPopUp.SetActive(false);
        enemyAttackPopUp.SetActive(true);
    }
}
