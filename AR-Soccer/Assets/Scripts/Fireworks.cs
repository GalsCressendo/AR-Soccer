using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
    [SerializeField] GameObject fireworkParticle;

    private void Awake()
    {
        GameObject fireWork = Instantiate(fireworkParticle, transform.position, transform.rotation);
        fireWork.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
