using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDisplayWindow : MonoBehaviour
{

    private GameObject displayPrefab;

    public static InventoryDisplay CreateDisplay (string buttonText, InventoryDisplay.voidDelegate2 plantAction, InventoryDisplay.voidDelegate cancelAction, List<PickableItem> pickables)
    {
        GameObject displayObject = Instantiate(DisplayManager.Instance.itemDisplayWindow, DisplayManager.Instance.displayParent);
        InventoryDisplay display = displayObject.GetComponent<InventoryDisplay>();

        display.gameObject.SetActive(true);
        display.ClearItems();
        display.Init(buttonText,plantAction,cancelAction, pickables);

        //switch input!

        return display;
    }


    public static void DestroyDisplay (InventoryDisplay display)
    {
        //switch input back

        Destroy(display.gameObject);
    }
}


//call from flower bed on interact
//create display
//keep reference to display
//when selected, destroy display


