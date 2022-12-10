using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContainer : MonoBehaviour
{
    public List<GameObject> units = new List<GameObject>();

    public List<Transform> GetAllUnitTransform(Transform exclude)
    {
        List<Transform> unitTransform = new List<Transform>();
        foreach(GameObject p in units)
        {
            if(p.transform == exclude)
            {
                continue;
            }

            unitTransform.Add(p.transform);

        }

        return unitTransform;
    }
}
