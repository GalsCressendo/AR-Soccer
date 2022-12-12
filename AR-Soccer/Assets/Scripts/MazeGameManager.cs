using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGameManager : MonoBehaviour
{
    public NavMeshSurface surface;
    [SerializeField] List<GameObject> mazes;
    [SerializeField] GameObject field;
    [SerializeField] GameObject ball;
    GameObject spawnedBall;

    public float range = 3f;

    private void Awake()
    {
        int rand = Random.Range(0, mazes.Count);
        GameObject maze = Instantiate(mazes[rand], mazes[rand].transform.position, mazes[rand].transform.rotation);
        maze.transform.SetParent(field.transform, false);
        surface.BuildNavMesh();
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
