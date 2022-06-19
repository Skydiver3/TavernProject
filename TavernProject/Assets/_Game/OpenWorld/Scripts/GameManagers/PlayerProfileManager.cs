using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlayerProfileManager
{
    public PlayerProfileData data;
    public void Load()
    {
        string saveFile = Application.persistentDataPath + "/PlayerProfileData.json";

        //read file
        if (File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<PlayerProfileData>(fileContents);
        }
    }

    public void Save()
    {
        string saveFile = Application.persistentDataPath + "/PlayerProfileData.json";
        string jsonString = JsonUtility.ToJson(data);

        //write file
        File.WriteAllText(saveFile, jsonString);
    }
}

public class PlayerProfileData
{
    public string name;
    public int[] villagerApproval;
    public int[] inventory;
}

