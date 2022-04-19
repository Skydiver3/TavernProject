using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    //input fields
    public ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    public ThirdPersonActionsAsset GetPlayerActionsAsset()
    {
        if (playerActionsAsset == null)
        {
            playerActionsAsset = new ThirdPersonActionsAsset();
        }
        return playerActionsAsset;
    }

    public void SetUIInput()
    {
        playerActionsAsset.Player.Disable();
        playerActionsAsset.UI.Enable();

        cinemachineBrain.enabled = false;
    }

    public void SetThirdPersonInput()
    {
        playerActionsAsset.UI.Disable();
        playerActionsAsset.Player.Enable();

        cinemachineBrain.enabled = true;
    }
}
