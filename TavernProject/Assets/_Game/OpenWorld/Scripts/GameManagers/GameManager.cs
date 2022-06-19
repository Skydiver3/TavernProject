using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    public Inventory inventory;
    public string testString = "test";
    public WorldSettings worldSettings;
    public PlayerInputManager playerInputManager;
    public ItemDatabase itemDatabase;

    private void Awake()
    {
        print("Wake gameManager");
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

    }

}
