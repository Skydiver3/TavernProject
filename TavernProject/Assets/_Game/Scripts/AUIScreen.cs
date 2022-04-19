using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUIScreen : MonoBehaviour
{
    private void OnEnable()
    {
        EnableUIInput();
    }
    private void OnDisable()
    {
        EnableThirdPersonInput();
    }

    public void EnableUIInput()
    {
        GameManager.Instance.playerInputManager.SetUIInput();
    }

    public void EnableThirdPersonInput()
    {
        GameManager.Instance.playerInputManager.SetThirdPersonInput();
    }
}
