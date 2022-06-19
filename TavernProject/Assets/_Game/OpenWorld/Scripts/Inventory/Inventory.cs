using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        GameManager.Instance.inventory = this;

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
        print("try pick "+item);
        if (item) PickItem(item);
    }

    //add item to inventory and clear message
    private void PickItem(PickableItem item)
    {
        _itemList.Add(GameManager.Instance.itemDatabase.GetItem(item.itemID).GetComponent<PickableItem>());
        Destroy(item);
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
        Instantiate(item.gameObject,_dropTransform.position,_dropTransform.rotation);

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


    private InventoryData data;
    public void Load()
    {
        string saveFile = Application.persistentDataPath + "/InventoryData.json";

        //read file
        if (File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<InventoryData>(fileContents);
        }
        foreach (int id in data.indexList)
        {
            _itemList.Add(GameManager.Instance.itemDatabase.GetItem(id).GetComponent<PickableItem>());
        }
    }

    public void Save()
    {
        if (data == null) data = new InventoryData();

        data.indexList = new int[_itemList.Count];
        for (int i = 0; i < _itemList.Count; i++)
        {
            data.indexList[i] = _itemList[i].itemID;
        }

        string saveFile = Application.persistentDataPath + "/InventoryData.json";
        string jsonString = JsonUtility.ToJson(data);

        //write file
        File.WriteAllText(saveFile, jsonString);
    }
}
public class InventoryData
{
    public int[] indexList;
}
