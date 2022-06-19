using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationManager : MonoBehaviour
{
    //save world state, plants, inventory

    public WorldStateManager worldState;
    Inventory inventory;

    private IEnumerator Start()
    {
        //load field states
        //load picked items
        while (inventory == null)
        {
            yield return null;
            inventory = GameManager.Instance.inventory;
        }
        //load inventory
        //inventory.loadData(data)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Save();
            print("saved");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
            print("loaded");
        }
    }

    private void Save()
    {
        worldState.Save();
        inventory.Save();
    }
    private void Load()
    {
        worldState.Load();
        inventory.Load();
    }

}

