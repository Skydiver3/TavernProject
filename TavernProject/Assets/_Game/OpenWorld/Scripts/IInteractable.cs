using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool GetInteractive();
    public void Interact();
    public string GetInteractionText();
}
