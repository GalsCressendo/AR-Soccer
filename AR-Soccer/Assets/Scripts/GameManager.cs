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

    public List<Camera> cameras;

    public GameObject playerPrefab;
    public GameObject playerContainer;

    public GameObject enemyContainer;
    public GameObject enemyPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = GetActiveCamera();
            if(cam == null)
            {
                cam = Camera.main;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            GameObject spawnedObject = null;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.tag);
                if(hit.transform.tag == PLAYER_AREA_TAG)
                {
                    spawnedObject = Instantiate(playerPrefab, hit.point, playerPrefab.transform.rotation);
                    spawnedObject.transform.SetParent(playerContainer.transform,true);
                    playerContainer.GetComponent<UnitContainer>().units.Add(spawnedObject);
                    spawnedObject.GetComponent<Attacker>().unitContainer = playerContainer.GetComponent<UnitContainer>();
                }
                else if(hit.transform.tag == ENEMY_AREA_TAG)
                {
                    spawnedObject = Instantiate(enemyPrefab, hit.point, enemyPrefab.transform.rotation);
                    spawnedObject.transform.SetParent(enemyContainer.transform,true);
                    enemyContainer.GetComponent<UnitContainer>().units.Add(spawnedObject);
                    spawnedObject.GetComponent<Attacker>().unitContainer = enemyContainer.GetComponent<UnitContainer>();
                }

                if (spawnedObject != null)
                {
                    if(spawnedObject.tag == ATTACKER_TAG)
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

    private Camera GetActiveCamera()
    {
        foreach(Camera cam in cameras)
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
        spawned.GetComponent<Attacker>().ReactiveAfterSpawn();
        Debug.Log("Attacker activated");

    }

    private IEnumerator ReactiveDefenderAfterSpawn(GameObject spawned)
    {
        yield return new WaitForSeconds(SPAWN_TIME);
        spawned.GetComponent<Defender>().ReactiveAfterSpawn();
        Debug.Log("Defender activated");
    }
}
