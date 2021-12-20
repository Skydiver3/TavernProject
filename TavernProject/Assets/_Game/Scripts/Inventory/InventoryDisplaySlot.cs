using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryDisplaySlot : MonoBehaviour
{
    private InventoryDisplay _parentDisplay;

    [SerializeField] private Image _itemImage;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private TextMeshProUGUI _itemText;
    [SerializeField] private string _defaultText;
    [SerializeField] private GameObject _showMenuButton;
    [SerializeField] private GameObject _hideMenuButton;

    private bool _active = false;
    [SerializeField] private UnityEvent _onHoverExit;

    public void Init(InventoryDisplay display)
    {
        _parentDisplay = display;
    }

    public void DisplayItem(PickableItem item)
    {
        if (item == null)
        {
            _itemImage.sprite = _defaultSprite;
            _itemText.text = _defaultText;
            _showMenuButton.SetActive(false);
            _hideMenuButton.SetActive(false);
            _active = false;
            return;
        }
        _itemImage.sprite = item.displaySprite;
        _itemText.text = item.name;
        _showMenuButton.SetActive(true);
        _hideMenuButton.SetActive(false);
        _active = true;
    }

    public void TriggerDrop()
    {
        _parentDisplay.RegisterDropClick(this);
    }

    public void HideItemMenu()
    {
        if (_active) _onHoverExit?.Invoke();
    }
}
