using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InventoryManager : BaseManager<InventoryManager>
{
    public ItemInfo[] InventoryItems = new ItemInfo[36];
    public Transform SlotsParent;
    public Slots targetSlots;
    public bool IsBagFull { private set; get; }
    public Dictionary<ItemType, int> ItemsDic = new Dictionary<ItemType, int>();

    public void SaveItem()
    {
        MySystem.Instance.nowUserData.items = InventoryItems;
    }
    public void Init()
    { if (!MySystem.IsInLevel())
        {
            MySystem.Instance.whenSaveAction +=SaveItem;
        }

        for (int i = 0; i < InventoryItems.Length; i++)
        {
            InventoryItems[i].Count = 0;
            InventoryItems[i].Durability = 0;
        }
        if (MySystem.Instance.nowUserData.items.Length != 0)
            InventoryItems = MySystem.getUserData().items;

      
    }

    public int AddItems(ItemInfo info)//在背包中添加多个物品
    {

        int counter = info.Count;
        bool TakeThing = false;
        for (int i = 0; i < info.Count; i++)
        {
            if (AddANewItem(info))//能添加
            {

                counter -= 1;
                TakeThing = true;
            }
            else
            {
                break;
            }
        }
        if (TakeThing == true)
        {
            SoundSystem.Instance.Play2Dsound("Pop");
        }
        EventMgr.Instance.EventTrigger("OnChangeInventory");
        return counter;//返回剩余不能装下的量
    }
    private bool AddANewItem(ItemInfo info)
    {

        int aaaa = 0;
        if (!CheckHasItem(info.type, ref aaaa))//找不到对应的
        {
            int slotID = 0;
            if (FindEmptySlot(ref slotID))
            {
                InventoryItems[slotID].AddNewItem(info);
                return true;
            }
        }
        else//找到对应的
        {
            int slotID = 0;
            CheckHasItem(info.type, ref slotID);
            InventoryItems[slotID].AddItem();
            return true;
        }
        return false;


    }//在背包中添加单个物品

    public void SubTargetSlotItem(int target)
    {
        if (InventoryItems[target].Count > 0)
        {
            InventoryItems[target].Count--;
            EventMgr.Instance.EventTrigger("OnChangeInventory");
        }
    }
    public void AddDefaultItem(ItemType type, int num)
    {
        ItemInfo newInfo = new ItemInfo(type,num,ResourceSystem.Instance.GetItem(type).MaxDurability);
        AddItems(newInfo);
    }//仅输入物品类型以及物品数量来增加物品
    public bool CheckHasItem(ItemType type, ref int num)
    {
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (InventoryItems[i].CheckCanAddItem(type))//BUG
            {
                num = i;
                return true;
            }
        }

        return false;

    }//检查背包中是否有对应物品(且能容下)，并通过引用传返回这个格子对应的ID
    public bool ThereIsItem(ItemType type, ref int num)
    {
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (InventoryItems[i].type == type)
            {
                num = i;
                return true;
            }
        }
        return false;

    }//检查背包中是否有对应物品,并通过引用传返回这个格子对应的ID
    public bool ThereIsItem(ItemType type)
    {
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (InventoryItems[i].type == type)
            {
             
                return true;
            }
        }
        return false;

    }
    public bool FindEmptySlot(ref int num)
    {
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (InventoryItems[i].IsEmpty())
            {
                num = i;
                return true;
            }
        }
        return false;
    }//检查背包是否有空位，并返回第一个空位的ID
    public void UpdateItemDic()//更新物品信息
    {
        ItemsDic.Clear();
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (InventoryItems[i].type != ItemType.Nothing)
            {
                if (!ItemsDic.ContainsKey(InventoryItems[i].type))//如果没有这个Key
                {
                    ItemsDic.Add(InventoryItems[i].type, InventoryItems[i].Count);
                }
                else
                {
                    ItemsDic[InventoryItems[i].type] += InventoryItems[i].Count;
                }
            }
        }
    }
    public bool isContainItem(ItemType itemtype)
    {
        return ItemsDic.ContainsKey(itemtype);
    }
    public int GetItemCount(ItemType type)
    {

        if (ItemsDic.ContainsKey(type))
        {
            return ItemsDic[type];
        }
        return 0;
    }//检查背包中是否有对应物品，并返回该物品的数量
    public void DeleteTargetItem(ItemType type)
    {
        int num = 0;
        if (ThereIsItem(type, ref num))
        {
            InventoryItems[num].Count -= 1;
            EventMgr.Instance.EventTrigger("OnChangeInventory");
        }

    }

    public bool Deleteitems(ItemInfo info)
    {
        UpdateItemDic();
        if (GetItemCount(info.type) >= info.Count)
        {
            for (int i = 0; i < info.Count; i++)
            {
                DeleteTargetItem(info.type);
            }
            return true;

        }



        return false;

    }
    public ItemInfo[] GetItemInfos()
    {
        return InventoryItems;
    }

    public BulletType HasArrow()
    {
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (InventoryItems[i].type == ItemType.箭)
            {
                return BulletType.Normal_Arrow;
            }
            if (InventoryItems[i].type == ItemType.箭_沙石)
            {
                return BulletType.Sand_Arrow;
            }

        }
        return BulletType.no;
    }
    

}
