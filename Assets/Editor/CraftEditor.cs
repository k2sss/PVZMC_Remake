using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using K2STools;
[CustomEditor(typeof(CraftCreator))]
public class CraftEditor : Editor
{
    private CraftCreator creator;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        creator = (CraftCreator)target;
        if(GUILayout.Button("Create"))
        {
            creator.Create();
        }
        if(GUILayout.Button("Log"))
        {
            creator.Log();
        }
    
        if(GUILayout.Button("³·Ïú"))
        {
            creator.Back();
        }
    }
}