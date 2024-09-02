using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(ItemCreator))]

public class ItemCreatorEditor : Editor
{
    [SerializeField]
    private int ItemID;
    private ItemCreator tar;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        tar = (ItemCreator)target;
        if(GUILayout.Button("创建物品"))
        {
            tar.CreateItem();
        }
    }

}
