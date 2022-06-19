using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldStateManager : MonoBehaviour
{
    private PickableItem[] _pickableInWorld;
    private PickablesData data;


    public void ClearWorld()
    {
        foreach (PickableItem item in FindObjectsOfType<PickableItem>())
        {
            Destroy(item.gameObject);
        }
    }

    private void SetData()
    {
        _pickableInWorld = FindObjectsOfType<PickableItem>();

        int length = _pickableInWorld.Length;

        data = new PickablesData();
        data.Ids = new int[length];
        data.positions = new Vector3[length];
        data.scales = new Vector3[length];
        data.rotations = new Quaternion[length];

        for (int i = 0; i < length; i++)
        {
            data.Ids[i] = _pickableInWorld[i].itemID;

            data.positions[i] = _pickableInWorld[i].gameObject.transform.position;
            data.scales[i] = _pickableInWorld[i].gameObject.transform.lossyScale;
            data.rotations[i] = _pickableInWorld[i].gameObject.transform.rotation;
        }
        //save data
    }
    private void PlacePickables()
    {
        ClearWorld();

        List<PickableItem> loadedPickables = new List<PickableItem>();

        for (int i = 0; i < data.Ids.Length; i++)
        {
            GameObject prefab = GameManager.Instance.itemDatabase.GetItem(data.Ids[i]);
            GameObject newObject = Instantiate(prefab, data.positions[i], data.rotations[i]);
            newObject.transform.localScale = data.scales[i];
        }

    }

    public void Load()
    {
        string saveFile = Application.persistentDataPath + "/PickablesData.json";

        //read file
        if (File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<PickablesData>(fileContents);
        }
        PlacePickables();
    }

    public void Save()
    {
        SetData();

        string saveFile = Application.persistentDataPath + "/PickablesData.json";
        string jsonString = JsonUtility.ToJson(data);

        //write file
        File.WriteAllText(saveFile, jsonString);
    }


}

public class PickablesData
{
    public int[] Ids;
    public Vector3[] positions;
    public Vector3[] scales;
    public Quaternion[] rotations;
}