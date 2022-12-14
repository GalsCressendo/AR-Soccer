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
    [SerializeField] GameObject ballSpawnParticle;
    [SerializeField] GameObject fireWork;

    public bool gameIsActive = false;

    float range = 3f;

    private void Start()
    {
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.HORN_SFX);
        Invoke("InitializeGame", FindObjectOfType<AudioManager>().GetAudioDuration(AudioManager.HORN_SFX));
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
        if (maze!=null && gameIsActive)
        {
            if (spawnedBall == null)
            {
                Vector3 point;
                if (RandomPoint(transform.position, range, out point))
                {
                    spawnedBall = Instantiate(ball, point, ball.transform.rotation);
                    spawnedBall.transform.SetParent(field.transform, true);
                    StartCoroutine(CreateSpawnBallParticle(spawnedBall.transform));
                }
            }
        }
       
    }

    private void InitializeGame() 
    {
        timer.isTicking = true;
        player.SetActive(true);
        Destroy(penaltyGamePopUp);
        gameIsActive = true;

        int rand = Random.Range(0, mazes.Count);
        maze = Instantiate(mazes[rand], mazes[rand].transform.position, mazes[rand].transform.rotation);
        maze.transform.SetParent(field.transform, false);
        surface.BuildNavMesh();
    }

    public void DeclareEnemyWinner()
    {
        gameIsActive = false;
        timer.isTicking = false;
        resultPopUp.transform.gameObject.SetActive(true);
        resultPopUp.SetEnemyWinner();
        Instantiate(fireWork, fireWork.transform.position, fireWork.transform.rotation);
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.APPLAUSE_SFX);
    }

    public void DeclarePlayerWinner()
    {
        gameIsActive = false;
        timer.isTicking = false;
        resultPopUp.transform.gameObject.SetActive(true);
        resultPopUp.SetPlayerWinner();
        Instantiate(fireWork, fireWork.transform.position, fireWork.transform.rotation);
        FindObjectOfType<AudioManager>().PlayAudio(AudioManager.APPLAUSE_SFX);
    }

    public void PauseGame()
    {
        timer.isTicking = false;
        gameIsActive = false;

    }

    public IEnumerator CreateSpawnBallParticle(Transform target)
    {
        GameObject effect = Instantiate(ballSpawnParticle, target.position, target.rotation);
        effect.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        yield return new WaitForSeconds(1f);
        Destroy(effect);
    }

}
