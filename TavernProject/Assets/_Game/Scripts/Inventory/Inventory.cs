using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<PickableItem> _itemList = new List<PickableItem>();

    public bool listeningForInput = true;
    [SerializeField] private Transform _dropTransform;

    private InventoryDisplay _display;


    private void Start()
    {
        GameManager.Instance.playerInputManager.GetPlayerActionsAsset().Player.OpenInventory.started += ShowInventory;

        _display = DisplayManager.Instance.inventoryDisplay;
        _display.onItemDrop += DropItem;
    }
    private void OnDestroy()
    {
        GameManager.Instance.playerInputManager.GetPlayerActionsAsset().Player.OpenInventory.started -= ShowInventory;
        _display.onItemDrop -= DropItem;
    }

    private void ShowInventory(InputAction.CallbackContext obj)
    {
        _display.gameObject.SetActive(true);
        _display.Init();
        _display.ClearItems();
        _display.DisplayItems(_itemList);
    }

    public void TryPickItem(PickableItem item)
    {
        if (item) PickItem(item);
    }

    //add item to inventory and clear message
    private void PickItem(PickableItem item)
    {
        _itemList.Add(item);
        item.Hide();
        PlayerMessageSystem.Instance.Hide("Pick " + item.name);
        //TODO: build proper menu view for item display. do we want a quick access bar?
        //_display.DisplayItemAt(item, _itemList.Count - 1);
    }

    //remove item from list, call drop event of item, tell display to update
    private void DropItem(int index, PickableItem item)
    {
        if (_itemList.Count <= index)
        {
            Debug.LogWarning("[Inventory] Attempted to remove item from empty slot, no item to remove at this index.");
            return;
        }
        _itemList.RemoveAt(index);
        item.Drop(_dropTransform);

        _display.ClearItems();
        _display.DisplayItems(_itemList);
    }

    public void RemoveItem(PickableItem item)
    {
        if (!_itemList.Contains(item)) return;

        _itemList.Remove(item);
    }

    public void RemoveItem(int i)
    {
        if (_itemList.Count <= i) return;

        _itemList.RemoveAt(i);
    }

    //filter items in inventory for one specific type
    public List<PickableItem> GetItemsOfType<T>() where T : PickableItem
    {
        List<PickableItem> items = new List<PickableItem>();
        foreach (PickableItem item in _itemList)
        {
            T castPickable = item as T;
            if (castPickable != null)
            {
                items.Add(item);
            }
        }

        return items;
    }
}
