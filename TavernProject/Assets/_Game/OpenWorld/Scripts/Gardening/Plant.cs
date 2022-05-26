using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Plants", fileName ="Plant")]
public class Plant : ScriptableObject
{
    //plant is watered for a globally set time
    //time needed until next stage, running while watered
    public int timeNeedGrow;

    //how many times the plant bears fruit
    public bool finite = true;
    public bool pickAll = false;
    public int cycles = 1;

    public GameObject plantPickablePrefab;
    public List<GameObject> plantStages;
}
