using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerArmor;
[CreateAssetMenu(fileName = "newArmorData",menuName ="ArmorData")]
public class ArmorDatas : ScriptableObject
{
    public PlayerArmor.PlayerArmorData[] datas;

    public ArmorGroup FindTheAromorGroup(ItemType itemtype,ref int parentID)
    {
        if (itemtype == ItemType.Nothing)
        {
            return null;
        }
        for (int i = 0; i < datas.Length; i++)
        {
            for (int j = 0; j < datas[i].groups.Length; j++)
            {
                if (datas[i].groups[j].targetItemType == itemtype)
                {
                    parentID = i;
                    return datas[i].groups[j];
                }
            }
        }
        return null;
    }
}
