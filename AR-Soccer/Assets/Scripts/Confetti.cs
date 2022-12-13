using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField] GameObject confettiParticle;

    public void GetConffeti(Transform targetTransform)
    {
        StartCoroutine(SpawnConfetti(targetTransform));
    }

    private IEnumerator SpawnConfetti(Transform targetTransform)
    {
        GameObject confetti = Instantiate(confettiParticle, targetTransform.position, targetTransform.rotation);
        confetti.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(FindObjectOfType<AudioManager>().GetAudioDuration(AudioManager.GOAL_SFX));
        Destroy(gameObject);
        Destroy(confetti);
    }
}
