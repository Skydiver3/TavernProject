using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DiaryData
{
    public string[] entries = new string[0];
}
public class DiaryDataManager
{
    public DiaryData data = new DiaryData();

    public void SetEntries(List<string> _entries)
    {
        data.entries = new string[_entries.Count];
        for (int i = 0; i < _entries.Count; i++)
        {
            data.entries[i] = _entries[i];
        }
    }

    public void AddEntry(string newEntry)
    {
        string[] newEntries = new string[data.entries.Length + 1];
        for (int i = 0; i < data.entries.Length; i++)
        {
            newEntries[i] = data.entries[i];
        }
        newEntries[newEntries.Length - 1] = newEntry;
        data.entries = newEntries;
    }

    public List<string> GetEntries()
    {
        return new List<string>(data.entries);
    }

    public void Load()
    {
        string saveFile = Application.persistentDataPath + "/gamedata.json";

        //read file
        if (File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<DiaryData>(fileContents);
        }
    }

    public void Save()
    {
        string saveFile = Application.persistentDataPath + "/gamedata.json";
        string jsonString = JsonUtility.ToJson(data);

        //write file
        File.WriteAllText(saveFile, jsonString);
    }
}
