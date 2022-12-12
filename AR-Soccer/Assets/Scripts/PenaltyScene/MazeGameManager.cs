using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGameManager : MonoBehaviour
{
    public NavMeshSurface surface;
    GameObject spawnedBall;
    GameObject maze;
    [SerializeField] List<GameObject> mazes;
    [SerializeField] GameObject field;
    [SerializeField] GameObject ball;
    [SerializeField] ResultPopUp resultPopUp;
    [SerializeField] Timer timer;
    [SerializeField] GameObject player;
    [SerializeField] GameObject penaltyGamePopUp;


    bool gameStart = false;
    float range = 3f;

    private void Awake()
    {
        Invoke("InitializeGame", 0.5f);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void Update()
    {
        if (gameStart && maze!=null)
        {
            if (spawnedBall == null)
            {
                Vector3 point;
                if (RandomPoint(transform.position, range, out point))
                {
                    spawnedBall = Instantiate(ball, point, ball.transform.rotation);
                    spawnedBall.transform.SetParent(field.transform, true);
                }
            }
        }
       
    }

    private void InitializeGame() 
    {
        timer.isTicking = true;
        player.SetActive(true);
        Destroy(penaltyGamePopUp);
        gameStart = true;

        int rand = Random.Range(0, mazes.Count);
        maze = Instantiate(mazes[rand], mazes[rand].transform.position, mazes[rand].transform.rotation);
        maze.transform.SetParent(field.transform, false);
        surface.BuildNavMesh();
    }

    public void DeclareEnemyWinner()
    {
        timer.isTicking = false;
        resultPopUp.transform.gameObject.SetActive(true);
        resultPopUp.SetEnemyWinner();
    }

    public void DeclarePlayerWinner()
    {
        timer.isTicking = false;
        resultPopUp.transform.gameObject.SetActive(true);
        resultPopUp.SetPlayerWinner();
    }

}
