using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChestSlot : MonoBehaviour
{
    public Slot[] slots;
    public Scriptable_Levelinfo[] levelinfo;

    private ItemInfo[] items;
    private void Start()
    {
        slots = new Slot[transform.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = transform.GetChild(i).gameObject.GetComponent<Slot>();
        }
        for (int i = 0; i < levelinfo.Length; i++)
        {
            items = levelinfo[i].GetItem(MySystem.Instance.nowUserData.LevelType, MySystem.Instance.nowUserData.levelName);
            if(items != null&&items.Length > 0)
            for (int j = 0; j < items.Length; j++)
            {
                AddItems(items[j]);
            }

        }

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
            //SoundSystem.Instance.Play2Dsound(pop);
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
                slots[slotID].AddNewItem(info);
                return true;
            }
        }
        else//找到对应的
        {
            int slotID = 0;
            CheckHasItem(info.type, ref slotID);
            slots[slotID].AddItem();
            return true;
        }
        return false;


    }//在背包中添加单个物品
    public bool CheckHasItem(ItemType type, ref int num)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].CheckCanAddItem(type))
            {
                num = i;
                return true;
            }
        }
        return false;

    }//检查背包中是否有对应物品(且能容下)，并通过引用传返回这个格子对应的ID
    public bool FindEmptySlot(ref int num)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                num = i;
                return true;
            }
        }
        return false;
    }//检查背包是否有空位，并返回第一个空位的ID
}
