using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelInfo", menuName = "newLevelInfo")]
public class Scriptable_Levelinfo : ScriptableObject
{
    public List<Scriptable_Levelinfo_Single> levels = new List<Scriptable_Levelinfo_Single>();
    public ItemInfo[] GetItem(degreetype type, string levelName)
    {
        bool first = false;
        first = !MySystem.Instance.nowUserData.GetLevelFinishData(levelName, type);
        ItemInfo[] items = null;
        for (int j = 0; j < levels.Count; j++)
        {
            if (levels[j].LevelName == levelName)
            {
                items = levels[j].GetItem(type);
            }
        }
        return items;
    }

}
[System.Serializable]
public class Scriptable_Levelinfo_Single
{
    public string LevelName;
    public int ExLevelNum = 2;
    public string LevelSceneName;
    public string LevelSceneName_hard;
    public string LevelSceneName_hell;
    public ItemInfo[] GetItem_normal_first;
    public ItemInfo[] GetItem_normal;
    public ItemInfo[] GetItem_hard_first;
    public ItemInfo[] GetItem_hard;
    public ItemInfo[] GetItem_hell_first;
    public ItemInfo[] GetItem_hell;

    [Multiline]
    public string Describe;
    [Multiline]
    public string Describe_hard_ex;
    [Multiline]
    public string Describe_hell_ex;
    public ItemInfo[] GetItem(degreetype type)
    {
        bool first = false;
        first = !MySystem.Instance.nowUserData.GetLevelFinishData(LevelName,type);
        ItemInfo[] items = null;
                if (first == false)
                {
                    if (type == degreetype.normal)
                        items = GetItem_normal;
                    if (type == degreetype.hard)
                        items = GetItem_hard;
                    if (type == degreetype.hell)
                        items = GetItem_hell;
                }
                else//首次获得
                {
                    if (type == degreetype.normal)
                        items = ItemCombine(GetItem_normal_first, GetItem_normal);
                    if (type == degreetype.hard)
                        items = ItemCombine(GetItem_hard_first, GetItem_hard);
                    if (type == degreetype.hell)
                        items = ItemCombine(GetItem_hell_first, GetItem_hell);
                }
        return items;
    }
    private ItemInfo[] ItemCombine(ItemInfo[] a, ItemInfo[] b)
    {
        ItemInfo[] go = new ItemInfo[a.Length + b.Length];
        for (int i = 0; i < a.Length; i++)
        {
            go[i] = a[i];
        }
        for (int i = a.Length; i < go.Length; i++)
        {
            go[i] = b[i - a.Length];
        }
        return go;
    }
}