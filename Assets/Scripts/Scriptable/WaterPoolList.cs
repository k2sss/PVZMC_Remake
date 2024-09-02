using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="poolName",menuName ="pool")]
public class WaterPoolList : ScriptableObject
{
    public FishingPoolObj[] poolObj;

    [ContextMenu("ShowIDname")]
    public void UpdateName()
    {
        foreach (FishingPoolObj item in poolObj)
        {
            item.ItemName = ((ItemType)item.ItemId).ToString();
        }
    }
}

[System.Serializable]
public class FishingPoolObj
{
    public string ItemName;
    [Tooltip("掉落物品ID")]
    public int ItemId;
    [Tooltip("掉落权重")]
    public int weight;

}