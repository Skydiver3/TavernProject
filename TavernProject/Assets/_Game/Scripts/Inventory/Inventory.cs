using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<PickableItem> _itemList = new List<PickableItem>();

    public bool listeningForInput = true;
    [SerializeField] string pickableTag = "pickableItem";
    [SerializeField] private Transform _dropTransform;

    private InventoryDisplay _display;

    private ThirdPersonActionsAsset playerActionsAsset;

    private PickableItem _hoveredItem;

    private void Awake()
    {

        playerActionsAsset = new ThirdPersonActionsAsset();
    }
    private void OnEnable()
    {
        playerActionsAsset.Player.Interact.started += TryPickItem;
        playerActionsAsset.Player.Enable();
    }
    private void Start()
    {
        _display = DisplayManager.Instance.inventoryDisplay;
        _display.onItemDrop += DropItem;
    }
    private void OnDisable()
    {
        playerActionsAsset.Player.Interact.started -= TryPickItem;
        playerActionsAsset.Player.Disable();
        
    }
    private void OnDestroy()
    {
        _display.onItemDrop -= DropItem;
    }
    private void OnTriggerExit(Collider other)
    {
        _hoveredItem = null;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != pickableTag) return;
        if (_hoveredItem == null)
        {
            PickableItem item = other.GetComponent<PickableItem>();
            if (!item) Debug.LogError("[Inventory] Object marked as Item has no Pickable attached, failed to pick up.");
            _hoveredItem = item;
        }
    }

    private void TryPickItem(InputAction.CallbackContext obj)
    {
        if (_hoveredItem) PickItem(_hoveredItem);
        _hoveredItem = null;
    }

    private void PickItem(PickableItem item)
    {
        _itemList.Add(item);
        item.Hide();
        //TODO: build proper menu view for item display. do we want a quick access bar?
        //_display.DisplayItemAt(item, _itemList.Count - 1);
    }

    private void DropItem(int index)
    {
        if(_itemList.Count<= index)
        {
            Debug.LogWarning("[Inventory] Attempted to remove item from empty slot, no item to remove at this index.");
            return;
        }
        PickableItem item = _itemList[index];
        _itemList.RemoveAt(index);
        item.PlaceAt(_dropTransform);

        _display.ClearItems();
        _display.DisplayItems(_itemList);
    }
}
