using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryDisplay : AUIScreen
{
    [SerializeField] private InventoryDisplaySlot[] _slots;
    [SerializeField] private Button _cancelButton;

    public delegate void voidDelegate();
    public delegate void voidDelegate2(int i, PickableItem item);
    
    public voidDelegate2 onItemDrop;
    public voidDelegate onCancel;

    private bool _initialized = false;

    private string _buttonText = "";
    private List<PickableItem> _displayedItems;

    private ThirdPersonActionsAsset playerActionsAsset;

    //init, display all items, set all button texts
    private void Start()
    {
        if (!_initialized) Init();

        if (_buttonText != "") SetButtonText(_buttonText);
        if (_displayedItems != null) DisplayItems(_displayedItems);

    }

    public void Show(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(true);
    }

    //register all slots
    public void Init()
    {
        _slots = GetComponentsInChildren<InventoryDisplaySlot>();
        foreach (InventoryDisplaySlot slot in _slots)
        {
            slot.Init(this);
        }
        _initialized = true;
    }

    //register slots, set custom drop event and button text, set filter for inventory list
    public void Init(string buttonText, voidDelegate2 dropAction, voidDelegate cancelAction, List<PickableItem> pickables)
    {
        Init();
        onItemDrop += dropAction;
        _buttonText = buttonText;
        _displayedItems = pickables;

        onCancel += cancelAction;
        _cancelButton.onClick.AddListener(CancelDisplay);
    }

    public void CancelDisplay()
    {
        onCancel?.Invoke();
    }

    //tell slots to display items from list
    public void DisplayItems(List<PickableItem> items)
    {
        if (items.Count > _slots.Length) Debug.LogError("[InventoryDisplay] More items than item slots!");

        //display every single item in registered slots
        for (int i = 0; i < items.Count; i++)
        {
            _slots[i].DisplayItem(items[i]);
        }

        _displayedItems = items;
    }

    //tell slots to clear items
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

    //tell a specific slot to drop item and call drop event
    public void RegisterDropClick(InventoryDisplaySlot slot)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (slot == _slots[i])
            {
                onItemDrop?.Invoke(i,_displayedItems[i]);
                return;
            }
        }
        Debug.LogError("[Inventory Display] Inventory Slot that called Drop event is not registered in Display Slot List.");
    }

    //set button text for all slots
    public void SetButtonText(string buttonText)
    {
        if (_slots.Length == 0) Debug.LogError("[Inventory Display] Trying to set button text but no slots found in display"); 
        foreach (InventoryDisplaySlot slot in _slots)
        {
            slot.SetButtonText(buttonText);
        }
    }
}
