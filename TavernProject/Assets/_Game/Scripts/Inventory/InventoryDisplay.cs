using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private InventoryDisplaySlot[] _slots;

    public delegate void voidDelegate(int i);
    public voidDelegate onItemDrop;

    private void Awake()
    {
        _slots = GetComponentsInChildren<InventoryDisplaySlot>();
        foreach (InventoryDisplaySlot slot  in _slots)
        {
            slot.Init(this);
        }
    }

    public void DisplayItems(List<PickableItem> items)
    {
        if (items.Count > _slots.Length) Debug.LogError("[InventoryDisplay] More items than item slots!");

        for (int i = 0; i < items.Count; i++)
        {
            _slots[i].DisplayItem(items[i]);
        }
    }
    public void ClearItems()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].DisplayItem(null);
        }
    }

    public void DisplayItemAt(PickableItem item, int index)
    {
        if (index >= _slots.Length) Debug.LogError("[InventoryDisplay] Invalid index when trying to display item");

        _slots[index].DisplayItem(item);
    }

    public void ClearAt(int index)
    {
        if (index >= _slots.Length) Debug.LogError("[InventoryDisplay] Invalid index when trying to clear item slot");

        _slots[index].DisplayItem(null);
    }

    public void RegisterDropClick(InventoryDisplaySlot slot)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (slot == _slots[i])
            {
                onItemDrop?.Invoke(i);
                return;
            }
        }
        Debug.LogError("[Inventory Display] Inventory Slot that called Drop event is not registered in Display Slot List.");
    }
}
