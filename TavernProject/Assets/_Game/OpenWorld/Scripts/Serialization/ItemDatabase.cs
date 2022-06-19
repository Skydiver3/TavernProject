using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<GameObject> items;

    private void Start()
    {
        FillDatabase();
    }
    public void FillDatabase()
    {
        //Debug.Log("Finding all Prefabs that have a PickableItem component...");

        string[] guids = AssetDatabase.FindAssets("t:Object", new[] { "Assets/_Game/OpenWorld/Prefabs" });

        items = new List<GameObject>();
        int i = 0;
        foreach (string guid in guids)
        {
            string myObjectPath = AssetDatabase.GUIDToAssetPath(guid);
            Object[] myObjs = AssetDatabase.LoadAllAssetsAtPath(myObjectPath);

            foreach (Object thisObject in myObjs)
            {
                if(thisObject is PickableItem)
                {
                    items.Add((thisObject as PickableItem).gameObject);

                    PickableItem item = thisObject as PickableItem;
                    item.itemID = i;
                    i++;
                }
            }
        }
    }
    public GameObject GetItem(int itemID)
    {
        if (items.Count > itemID) return items[itemID];
        return null;
    }
}
