using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemInfo : ICloneable
{
    public ItemType type;
    public int Count = 0;
    public int Durability = 0;
    public bool CheckCanAddItem(ItemType type)
    {
        if (this.type == type && Count < ResourceSystem.Instance.GetItem(type).MaxAmount)
        {
            return true;
        }
        return false;
    }
    public bool IsEmpty()
    {
        if (type == ItemType.Nothing)
        {
            return true;
        }
        return false;
    }//检查这个Slot是否为空
    public void AddNewItem(ItemInfo info)//添加一个新物品
    {
        type = info.type;
        Count += 1;
        Durability = info.Durability;
    }
    public void AddItem()//添加一个已有的物品
    {
        Count += 1;
    }
    public object Clone()
    {
        return this.MemberwiseClone();  //浅拷贝
    }

    //浅拷贝  
    public ItemInfo ShallowClone()
    {
        return this.Clone() as ItemInfo;
    }
    public ItemInfo(ItemType type,int count,int dur)
    {
        this.type = type;
        Count = count;
        Durability = dur;
    }

}
[System.Serializable]
public class ItemText
{
    public string text;
    public Color color = Color.white;
}