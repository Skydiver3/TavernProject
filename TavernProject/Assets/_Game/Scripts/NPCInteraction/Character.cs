using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/Character")]
public class Character : ScriptableObject
{
    public string description;
    public string[] lines;
    public Sprite image;
}
