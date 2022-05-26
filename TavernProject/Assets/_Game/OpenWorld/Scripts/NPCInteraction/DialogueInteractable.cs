using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    public Character character;
    public bool interactive = true;
    private static string _interactionText = "Talk to ";

    public string GetInteractionText()
    {
        return _interactionText + character.name;
    }

    public bool GetInteractive()
    {
        return interactive;
    }

    private string GetLine(int l)
    {
        return character.lines[l];
    }

    public void Interact()
    {
        print("character: " + character.name);
        DisplayManager.Instance.dialogueWindow.DisplayDialogue(character);
    }
}
