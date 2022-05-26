using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject[] onTriggerHide;
    public GameObject[] onTriggerShow;

    private void OnTriggerEnter(Collider other)
    {
        if (onTriggerHide != null)
        {
            foreach (GameObject item in onTriggerHide)
            {
                item.SetActive(false);
            }
        }
        if (onTriggerShow != null)
        {
            foreach (GameObject item in onTriggerShow)
            {
                item.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (onTriggerHide != null)
        {
            foreach (GameObject item in onTriggerHide)
            {
                item.SetActive(true);
            }
        }
        if (onTriggerShow != null)
        {
            foreach (GameObject item in onTriggerShow)
            {
                item.SetActive(false);
            }
        }
    }
}
