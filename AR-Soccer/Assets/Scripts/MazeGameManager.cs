using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGameManager : MonoBehaviour
{
    public NavMeshSurface surface;
    [SerializeField] List<GameObject> mazes;
    [SerializeField] GameObject field;

    private void Awake()
    {
        int rand = Random.Range(0, mazes.Count);
        GameObject maze = Instantiate(mazes[rand], mazes[rand].transform.position, mazes[rand].transform.rotation);
        maze.transform.SetParent(field.transform, false);
        surface.BuildNavMesh();
    }
}
