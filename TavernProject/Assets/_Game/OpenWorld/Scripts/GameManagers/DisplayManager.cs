using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    //TODO: make this a scriptable object singleton!
    private static DisplayManager _instance;
    public static DisplayManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
        }
    }

    public InventoryDisplay inventoryDisplay;
    public Transform displayParent;
    public GameObject itemDisplayWindow;
    public InteractionScreen dialogueWindow;
}
