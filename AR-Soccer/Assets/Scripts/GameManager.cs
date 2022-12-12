using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    const string PLAYER_AREA_TAG = "PlayerArea";
    const string ENEMY_AREA_TAG = "EnemyArea";
    const string ATTACKER_TAG = "Attacker";
    const string DEFENDER_TAG = "Defender";
    const string BALL_TAG = "Ball";
    const int ATTACKER_SPAWN_COST = 2;
    const int DEFENDER_SPAWN_COST = 3;
    const float SPAWN_TIME = 0.5f;
    const float STATE_POP_UP_TIME = 1f;
    const int TOTAL_MATCH = 5;
    const string RUN_ANIM_PARAM = "isRunning";

    [SerializeField] List<Camera> cameras;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerContainer;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject ballPrefab;

    [SerializeField] BoxCollider playerAreaCollider;
    [SerializeField] BoxCollider enemyAreaCollider;

    [SerializeField] Timer timer;

    [SerializeField] EnergyBar energyBarPlayer;
    [SerializeField] EnergyBar energyBarEnemy;

    public StatePopUp statePopUp;
    public bool gameIsActive;

    int matchCount = 1;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] ResultPopUp resultPopUp;


    public enum GameState
    {
        NONE,
        PLAYER_ATTACK_STATE,
        PLAYER_DEFENSE_STATE
    }

    [SerializeField] TextMeshProUGUI playerStateText;
    [SerializeField] TextMeshProUGUI enemyStateText;

    public GameState state = GameState.NONE;

    private void Start()
    {
        EnableStatePopUp();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameIsActive)
            {
                Camera cam = GetActiveCamera();
                if (cam == null)
                {
                    cam = Camera.main;
                }

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                GameObject spawnedObject = null;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == PLAYER_AREA_TAG)
                    {
                        if(state == GameState.PLAYER_ATTACK_STATE)
                        {
                            if(energyBarPlayer.activePoints >= ATTACKER_SPAWN_COST)
                            {
                                spawnedObject = SpawnPlayer(hit);
                                SetUnitAttackState(spawnedObject, true);
                                energyBarPlayer.ReduceEnergy(ATTACKER_SPAWN_COST);
                            }
                        }
                        else
                        {
                            if(energyBarPlayer.activePoints >= DEFENDER_SPAWN_COST)
                            {
                                spawnedObject = SpawnPlayer(hit);
                                SetUnitAttackState(spawnedObject, false);
                                energyBarPlayer.ReduceEnergy(DEFENDER_SPAWN_COST);
                            }

                        }
                        
                    }
                    else if (hit.transform.tag == ENEMY_AREA_TAG)
                    {

                        if(state == GameState.PLAYER_DEFENSE_STATE)
                        {
                            if(energyBarEnemy.activePoints >= ATTACKER_SPAWN_COST)
                            {
                                spawnedObject = SpawnEnemy(hit);
                                SetUnitAttackState(spawnedObject, true);
                                energyBarEnemy.ReduceEnergy(ATTACKER_SPAWN_COST);
                            }
                        }
                        else
                        {
                            if(energyBarEnemy.activePoints >= DEFENDER_SPAWN_COST)
                            {
                                spawnedObject = SpawnEnemy(hit);
                                SetUnitAttackState(spawnedObject, false);
                                energyBarEnemy.ReduceEnergy(DEFENDER_SPAWN_COST);
                            }

                        }
                    }

                    if (spawnedObject != null)
                    {
                        if (spawnedObject.tag == ATTACKER_TAG)
                        {
                            StartCoroutine(ReactiveAttackerAfterSpawn(spawnedObject));
                        }
                        else if (spawnedObject.tag == DEFENDER_TAG)
                        {
                            StartCoroutine(ReactiveDefenderAfterSpawn(spawnedObject));
                        }
                    }
                }

            }
        }

    }

    private Camera GetActiveCamera()
    {
        foreach (Camera cam in cameras)
        {
            if (cam.isActiveAndEnabled)
            {
                return cam;
            }
        }

        return null;
    }

    private IEnumerator ReactiveAttackerAfterSpawn(GameObject spawned)
    {
        yield return new WaitForSeconds(SPAWN_TIME);

        if (spawned.activeSelf)
        {
            spawned.GetComponent<Attacker>().ReactiveAfterSpawn();
        }

    }

    private IEnumerator ReactiveDefenderAfterSpawn(GameObject spawned)
    {
        yield return new WaitForSeconds(SPAWN_TIME);

        if (spawned.activeSelf)
        {
            spawned.GetComponent<Defender>().ReactiveAfterSpawn();
        }

    }

    public void SpawnBall()
    {
        if (state == GameState.PLAYER_ATTACK_STATE)
        {
            CountSpawnBallArea(playerAreaCollider);
        }
        else if (state == GameState.PLAYER_DEFENSE_STATE)
        {
            CountSpawnBallArea(enemyAreaCollider);
        }


    }

    private void CountSpawnBallArea(BoxCollider areaCollider)
    {
        Transform areaTransform = areaCollider.GetComponent<Transform>();
        Vector3 center = areaTransform.position;

        Vector3 areaSize;
        areaSize.x = areaTransform.localScale.x * areaCollider.size.x;
        areaSize.z = areaTransform.localScale.z * areaCollider.size.z;

        Vector3 randomPosition = new Vector3(Random.Range(-areaSize.x / 2, areaSize.x / 2), 0f, Random.Range(-areaSize.z / 2, areaSize.z / 2));
        Vector3 spawnPosition = new Vector3(center.x + randomPosition.x, 0.2f, center.z + randomPosition.z);

        GameObject ball = Instantiate(ballPrefab, spawnPosition, ballPrefab.transform.rotation);

    }

    private void EnableStatePopUp()
    {
        statePopUp.transform.gameObject.SetActive(true);

        //Destroy ball if exist
        if (GameObject.FindGameObjectWithTag(BALL_TAG))
        {
            Destroy(GameObject.FindGameObjectWithTag(BALL_TAG));
        }


        if (state == GameState.PLAYER_ATTACK_STATE)
        {
            statePopUp.ShowPlayerAttack();
        }
        else if (state == GameState.PLAYER_DEFENSE_STATE)
        {
            statePopUp.ShowEnemyAttack();
        }

        energyBarEnemy.ResetEnergyBar();
        energyBarPlayer.ResetEnergyBar();

        Invoke("DisableStatePopUp", STATE_POP_UP_TIME);
    }

    private void DisableStatePopUp()
    {
        gameIsActive = true;
        SpawnBall();
        statePopUp.transform.gameObject.SetActive(false);
        timer.ResetTime();

        energyBarEnemy.StartEnergyBar();
        energyBarPlayer.StartEnergyBar();
    }

    public void SwitchGameState()
    {
        matchCount += 1;
        gameIsActive = false;
        SetAnimationsToIdle();

        if (matchCount <= TOTAL_MATCH)
        {
            if (state == GameState.PLAYER_ATTACK_STATE)
            {
                state = GameState.PLAYER_DEFENSE_STATE;
                playerStateText.text = "Player (DEFENDER)";
                enemyStateText.text = "Enemy (ATTACKER)";
            }
            else
            {
                state = GameState.PLAYER_ATTACK_STATE;
                playerStateText.text = "Player (ATTACKER)";
                enemyStateText.text = "Enemy (DEFENDER)";
            }

            Invoke("ClearStage", 1.5f);
        }
        else if (matchCount > TOTAL_MATCH)
        {
            DeclareGameWinner();
        }
        
    }

    private void ClearStage()
    {
        foreach (GameObject obj in playerContainer.GetComponent<UnitContainer>().units)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in enemyContainer.GetComponent<UnitContainer>().units)
        {
            Destroy(obj);
        }

        EnableStatePopUp();
    }

    private void SetUnitAttackState(GameObject unit, bool isAttacking)
    {
        if(isAttacking)
        {
            unit.tag = ATTACKER_TAG;
            unit.GetComponent<Attacker>().enabled = true;
            unit.GetComponent<Defender>().enabled = false;
        }
        else
        {
            unit.tag = DEFENDER_TAG;
            unit.GetComponent<Attacker>().enabled = false;
            unit.GetComponent<Defender>().enabled = true;
        }
    }

    private GameObject SpawnPlayer(RaycastHit hit)
    {
        GameObject spawnedObject;
        spawnedObject = Instantiate(playerPrefab, hit.point, playerPrefab.transform.rotation);
        spawnedObject.transform.SetParent(playerContainer.transform, true);
        playerContainer.GetComponent<UnitContainer>().units.Add(spawnedObject);
        spawnedObject.GetComponent<Attacker>().unitContainer = playerContainer.GetComponent<UnitContainer>();

        return spawnedObject;
    }

    private GameObject SpawnEnemy(RaycastHit hit)
    {
        GameObject spawnedObject;
        spawnedObject = Instantiate(enemyPrefab, hit.point, enemyPrefab.transform.rotation);
        spawnedObject.transform.SetParent(enemyContainer.transform, true);
        enemyContainer.GetComponent<UnitContainer>().units.Add(spawnedObject);
        spawnedObject.GetComponent<Attacker>().unitContainer = enemyContainer.GetComponent<UnitContainer>();

        return spawnedObject;
    }

    public void DeclareWinAttacker()
    {
        if(state == GameState.PLAYER_ATTACK_STATE)
        {
            scoreManager.AddPlayerScore();
        }
        else
        {
            scoreManager.AddEnemyScore();
        }

        SwitchGameState();

    }

    public void DeclareWinDefender()
    {
        if(state == GameState.PLAYER_DEFENSE_STATE)
        {
            scoreManager.AddPlayerScore();
        }
        else
        {
            scoreManager.AddEnemyScore();
        }

        SwitchGameState();
    }

    public void DeclareGameWinner()
    {
        state = GameState.NONE;

        energyBarEnemy.ResetEnergyBar();
        energyBarPlayer.ResetEnergyBar();

        bool playerWin = scoreManager.isPlayerWinner();
        resultPopUp.transform.gameObject.SetActive(true);

        if (!playerWin)
        {
            bool isDraw = scoreManager.isResultDraw();
            if (isDraw)
            {
                resultPopUp.SetResultDraw();
                //TO DO: CREATE BONUS STAGE
            }
            else
            {
                //player is lose, the enemy win
                resultPopUp.SetEnemyWinner();

            }

        }
        else //player is winner
        {
            resultPopUp.SetPlayerWinner();
        }

    }

    private void SetAnimationsToIdle()
    {
        foreach(Transform obj in playerContainer.transform)
        {
            obj.GetComponent<Animator>().SetBool(RUN_ANIM_PARAM, false);
        }

        foreach (Transform obj in enemyContainer.transform)
        {
            obj.GetComponent<Animator>().SetBool(RUN_ANIM_PARAM, false);
        }
    }

}
