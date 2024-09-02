using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutPutSlot : Slot
{
    public CraftTable table;
    public override void Update()
    {
        if (IsEnter == true)
        {
            HasInputMouseButton = false;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (HasInputMouseButton == false)
                {
                    TakeItemAllFast();
                }
            }
            else

            if (HasInputMouseButton == false)
            {
                ClickStack();
                TakeItemAll();
            }
        }
        AnimationSprite();
    }
    public void TakeItemAll()
    {
        if (Input.GetMouseButtonDown(0) && MyItemDrag.IsEmpty()&&info.type != ItemType.Nothing)
        {
            MyItemDrag.info = info.ShallowClone();
            CleanItem();
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            HasInputMouseButton = true;
            onClick?.Invoke();
            table.CleanOnce();
            table.CheckSlots();
            EventMgr.Instance.EventTrigger("OnChangeInventory");
        }
    }
    public void ClickStack()
    {
        if (info.type!= ItemType.Nothing&&(Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1) )&& MyItemDrag.info.type == info.type&&info.Count + MyItemDrag.info.Count <= ResourceSystem.Instance.GetItem(info).MaxAmount)
        {
            
            for(int i =0;i<info.Count;i++)
                MyItemDrag.AddItem();
            CleanItem();
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            HasInputMouseButton = true;
            onClick?.Invoke();
            table.CleanOnce();
            table.CheckSlots();
            EventMgr.Instance.EventTrigger("OnChangeInventory");
        }
    }
    public void TakeItemAllFast()
    {
        if (Input.GetMouseButtonDown(0)&&info.type!=ItemType.Nothing)
        {
            //Debug.Log(table.GetMaxOutPutCount());
            int num = table.GetMaxOutPutCount();
            info.Count = num * info.Count;
            int rest = InventoryManager.Instance.AddItems(info);
            if (rest > 0)
            {
                ItemInfo i = new ItemInfo(ItemType.Nothing,0,0);
                i = info.ShallowClone();
                i.Count = rest;
                PlayerItemManager.Instance.ThrowStuff(i);
            }
            CleanItem();
            ItemUpdate();
            HasInputMouseButton = true;
            for(int i =0;i<num;i++)
            table.CleanOnce();
            EventMgr.Instance.EventTrigger("OnChangeInventory");
        }
    }
    public void ShowItem(ItemType type,int num)
    {
        info.type = type;
        info.Count = num;
        info.Durability = ResourceSystem.Instance.GetItem(type).MaxDurability;
        ItemUpdate();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        CleanItem();
        ItemUpdate();
    }
}
