using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const string PLAYER_AREA_TAG = "PlayerArea";
    const string ENEMY_AREA_TAG = "EnemyArea";
    const string ATTACKER_TAG = "Attacker";
    const string DEFENDER_TAG = "Defender";
    const float SPAWN_TIME = 0.5f;
    const float STATE_POP_UP_TIME = 1f;

    [SerializeField] List<Camera> cameras;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerContainer;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject ballPrefab;

    [SerializeField] BoxCollider playerAreaCollider;
    [SerializeField] BoxCollider enemyAreaCollider;

    [SerializeField] Timer timer;

    public StatePopUp statePopUp;
    public bool gameIsActive;

    public enum GameState
    {
        NONE,
        PLAYER_ATTACK_STATE,
        PLAYER_DEFENSE_STATE
    }

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
                        spawnedObject = Instantiate(playerPrefab, hit.point, playerPrefab.transform.rotation);
                        spawnedObject.transform.SetParent(playerContainer.transform, true);
                        playerContainer.GetComponent<UnitContainer>().units.Add(spawnedObject);
                        spawnedObject.GetComponent<Attacker>().unitContainer = playerContainer.GetComponent<UnitContainer>();

                        if(state == GameState.PLAYER_ATTACK_STATE)
                        {
                            SetUnitAttackState(spawnedObject, true);
                        }
                        else
                        {
                            SetUnitAttackState(spawnedObject, false);
                        }
                    }
                    else if (hit.transform.tag == ENEMY_AREA_TAG)
                    {
                        spawnedObject = Instantiate(enemyPrefab, hit.point, enemyPrefab.transform.rotation);
                        spawnedObject.transform.SetParent(enemyContainer.transform, true);
                        enemyContainer.GetComponent<UnitContainer>().units.Add(spawnedObject);
                        spawnedObject.GetComponent<Attacker>().unitContainer = enemyContainer.GetComponent<UnitContainer>();

                        if(state == GameState.PLAYER_DEFENSE_STATE)
                        {
                            SetUnitAttackState(spawnedObject, true);
                        }
                        else
                        {
                            SetUnitAttackState(spawnedObject, false);
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
        Vector3 spawnPosition = new Vector3(center.x + randomPosition.x, 0.3f, center.z + randomPosition.z);

        Instantiate(ballPrefab, spawnPosition, ballPrefab.transform.rotation);

    }

    private void EnableStatePopUp()
    {
        statePopUp.transform.gameObject.SetActive(true);
        gameIsActive = false;
        if (state == GameState.PLAYER_ATTACK_STATE)
        {
            statePopUp.ShowPlayerAttack();
        }
        else if (state == GameState.PLAYER_DEFENSE_STATE)
        {
            statePopUp.ShowEnemyAttack();
        }

        Invoke("DisableStatePopUp", STATE_POP_UP_TIME);
    }

    private void DisableStatePopUp()
    {
        gameIsActive = true;
        SpawnBall();
        statePopUp.transform.gameObject.SetActive(false);
        timer.ResetTime();
    }

    public void SwitchGameState()
    {
        gameIsActive = false;
        ClearStage();

        if (state == GameState.PLAYER_ATTACK_STATE)
        {
            state = GameState.PLAYER_DEFENSE_STATE;
        }
        else
        {
            state = GameState.PLAYER_ATTACK_STATE;
        }

        EnableStatePopUp();
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
}
