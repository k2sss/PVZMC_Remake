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

    public int AddItems(ItemInfo info)//�ڱ�������Ӷ����Ʒ
    {

        int counter = info.Count;
        bool TakeThing = false;
        for (int i = 0; i < info.Count; i++)
        {
            if (AddANewItem(info))//�����
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
        return counter;//����ʣ�಻��װ�µ���
    }
    private bool AddANewItem(ItemInfo info)
    {

        int aaaa = 0;
        if (!CheckHasItem(info.type, ref aaaa))//�Ҳ�����Ӧ��
        {
            int slotID = 0;
            if (FindEmptySlot(ref slotID))
            {
                InventoryItems[slotID].AddNewItem(info);
                return true;
            }
        }
        else//�ҵ���Ӧ��
        {
            int slotID = 0;
            CheckHasItem(info.type, ref slotID);
            InventoryItems[slotID].AddItem();
            return true;
        }
        return false;


    }//�ڱ�������ӵ�����Ʒ

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
    }//��������Ʒ�����Լ���Ʒ������������Ʒ
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

    }//��鱳�����Ƿ��ж�Ӧ��Ʒ(��������)����ͨ�����ô�����������Ӷ�Ӧ��ID
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

    }//��鱳�����Ƿ��ж�Ӧ��Ʒ,��ͨ�����ô�����������Ӷ�Ӧ��ID
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
    }//��鱳���Ƿ��п�λ�������ص�һ����λ��ID
    public void UpdateItemDic()//������Ʒ��Ϣ
    {
        ItemsDic.Clear();
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (InventoryItems[i].type != ItemType.Nothing)
            {
                if (!ItemsDic.ContainsKey(InventoryItems[i].type))//���û�����Key
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
    }//��鱳�����Ƿ��ж�Ӧ��Ʒ�������ظ���Ʒ������
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
            if (InventoryItems[i].type == ItemType.��)
            {
                return BulletType.Normal_Arrow;
            }
            if (InventoryItems[i].type == ItemType.��_ɳʯ)
            {
                return BulletType.Sand_Arrow;
            }

        }
        return BulletType.no;
    }
    

}
