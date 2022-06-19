using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableItem : MonoBehaviour, IInteractable
{
    public string description = "An item. What is it? We don't know.";
    public Sprite displaySprite;
    public int itemID;

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Drop(Transform newTransform)
    {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
        gameObject.SetActive(true);
    }

    public void Interact()
    {
        Hide();
        GameManager.Instance.inventory.TryPickItem(this);
    }

    public string GetInteractionText()
    {
        return "Pick " + this.name;
    }

    public bool GetInteractive()
    {
        return true;
    }

}
