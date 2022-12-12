using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitAttributes", order = 1)]
public class UnitAttributes : ScriptableObject
{
    public string GOAL_TAG;
    public string FENCE_TAG;
    public Material inactiveMaterial;
}
