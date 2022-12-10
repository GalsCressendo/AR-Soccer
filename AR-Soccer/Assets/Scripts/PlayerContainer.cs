using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public List<GameObject> players; 

    public List<Transform> GetAllPlayersTransform(Transform exclude)
    {
        List<Transform> playersTransform = new List<Transform>();
        foreach(GameObject p in players)
        {
            if(p.name == exclude.name)
            {
                continue;
            }

            playersTransform.Add(p.transform);

        }

        return playersTransform;
    }
}
