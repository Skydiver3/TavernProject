using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDatabase))]
[CanEditMultipleObjects]
public class ItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        ItemDatabase itemDatabase = (ItemDatabase)target;
        if (GUILayout.Button("Fill Database"))
        {
            itemDatabase.FillDatabase();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
