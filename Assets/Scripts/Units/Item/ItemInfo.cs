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
    }//������Slot�Ƿ�Ϊ��
    public void AddNewItem(ItemInfo info)//���һ������Ʒ
    {
        type = info.type;
        Count += 1;
        Durability = info.Durability;
    }
    public void AddItem()//���һ�����е���Ʒ
    {
        Count += 1;
    }
    public object Clone()
    {
        return this.MemberwiseClone();  //ǳ����
    }

    //ǳ����  
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