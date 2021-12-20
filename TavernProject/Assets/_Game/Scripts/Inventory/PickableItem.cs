using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableItem : MonoBehaviour
{
    public string description = "An item. What is it? We don't know.";
    public Sprite displaySprite;

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void PlaceAt(Transform newTransform)
    {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
        gameObject.SetActive(true);
    }

}
