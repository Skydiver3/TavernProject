using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }



    public string testString = "test";


    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

    }

}
