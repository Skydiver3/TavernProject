using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMessageSystem : MonoBehaviour
{
    private static PlayerMessageSystem _instance;
    public static PlayerMessageSystem Instance { get { return _instance; } }

    public TextMeshProUGUI displayText;
    public GameObject displayObject;

    private Coroutine hideTextCoroutine;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Message(string msg, float seconds = 0.0f)
    {
        if (hideTextCoroutine != null) StopCoroutine(hideTextCoroutine);
        displayText.text = msg;
        if (seconds > 0) hideTextCoroutine = StartCoroutine(HideMessageAfterSeconds(seconds));
    }
    public void Hide(string s = "")
    {
        if (s != "" && s != displayText.text) return;

        displayText.text = "";
        displayObject.SetActive(true);
    }
    private IEnumerator HideMessageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Hide();
    }
}
