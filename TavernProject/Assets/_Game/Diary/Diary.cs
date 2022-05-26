using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour
{
    private DiaryDataManager diaryData = new DiaryDataManager();
    public TMP_InputField inputField;
    public Button arrowNextButton;
    public Button arrowBackButton;

    private string[] diaryEntries;
    private string newEntry;
    private int progress = 0;


    void Start()
    {
        LoadEntries();
        progress = diaryEntries.Length;
        if (diaryEntries.Length == 0) arrowBackButton.interactable = false;
        arrowNextButton.interactable = false;
    }


    public void LoadEntries()
    {
        diaryData.Load();
        diaryEntries = diaryData.data.entries;
    }
    public void DisplayAt(int i)
    {
        if (i <= diaryEntries.Length - 1)
        {
            inputField.text = diaryEntries[i];
            progress = i;
        }
        if (i == diaryEntries.Length)
        {
            inputField.text = "";
            progress = i;
        }
    }

    public void DisplayNext()
    {
        progress++;
        DisplayAt(progress);
        if (progress == diaryEntries.Length)
        {
            arrowNextButton.interactable = false;
            inputField.interactable = true;
            inputField.text = newEntry;
        }
        arrowBackButton.interactable = true;
    }

    public void DisplayPrevious()
    {
        if (progress == diaryEntries.Length) newEntry = inputField.text;
        progress--;
        DisplayAt(progress);
        if (progress == 0) arrowBackButton.interactable = false;
        arrowNextButton.interactable = true;
        inputField.interactable = false;

    }

    public void FinishSession()
    {
        if (progress == diaryEntries.Length) newEntry = inputField.text;        
        if (newEntry!=null && newEntry != "") diaryData.AddEntry(newEntry);
        diaryData.Save();
    }
}
