using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private IInteractable hoveredObject;

    private ThirdPersonActionsAsset playerActionsAsset;

    private bool _initialized = false;

    private void Start()
    {
        Subscribe();
    }
    private void OnEnable()
    {
        //only subscribe if Start has been called at least once
        if (_initialized) Subscribe();
    }
    private void Subscribe()
    {
        playerActionsAsset = GameManager.Instance.playerInputManager.playerActionsAsset;

        playerActionsAsset.Player.Interact.started += Interact;
        _initialized = true;
    }
    private void OnDisable()
    {
        playerActionsAsset.Player.Interact.started -= Interact;
    }

    //detect hover object and display message
    private void OnTriggerStay(Collider other)
    {
        //if existing one suddenly stops being interactive: hide
        if (hoveredObject != null && !hoveredObject.GetInteractive())
        {
            PlayerMessageSystem.Instance.Hide(hoveredObject.GetInteractionText());
        }

        
        //skip if one exists, but update message if suddenly changed
        if (hoveredObject != null)
        {
            if (PlayerMessageSystem.Instance.currentMessage != "" && hoveredObject.GetInteractionText() != PlayerMessageSystem.Instance.currentMessage)
            {
                PlayerMessageSystem.Instance.Message(hoveredObject.GetInteractionText());
            }
            return;
        }

        //show if new one entered and interactive
        IInteractable newInteractable = other.GetComponent<IInteractable>();
        if (newInteractable == null) return;
        if (!newInteractable.GetInteractive())return;

        hoveredObject = newInteractable;
        PlayerMessageSystem.Instance.Message(hoveredObject.GetInteractionText());
    }

    //clear hover object and clear message
    private void OnTriggerExit(Collider other)
    {
        if (hoveredObject != null) PlayerMessageSystem.Instance.Hide(hoveredObject.GetInteractionText());
        hoveredObject = null;
    }

    //interact with hover object if one has been detected in OnTriggerStay
    private void Interact(InputAction.CallbackContext obj)
    {
        if (hoveredObject == null) return;
        hoveredObject.Interact();

        //if message changed after interaction: show new message, otherwise hide message
        string _text = hoveredObject.GetInteractionText();
        if (PlayerMessageSystem.Instance.currentMessage!=""&&_text != PlayerMessageSystem.Instance.currentMessage)
        {
            PlayerMessageSystem.Instance.Message(hoveredObject.GetInteractionText());
        }
        else
        {
            PlayerMessageSystem.Instance.Hide(_text);
            hoveredObject = null;
        }

    }
}
