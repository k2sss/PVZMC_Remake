using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTable : MonoBehaviour
{
    private CraftSlot[] slots;
    private Transform SlotsParent;
    public ItemType[] types { get; private set; }
    public OutPutSlot outputSlot { private set; get; }
    public CraftBook craftBook { private set; get; }

    

    private void Awake()
    {
        SlotsParent = transform.Find("CraftTableSlots").transform;
        outputSlot = transform.Find("CraftTableOutPut").GetComponent<OutPutSlot>();
        outputSlot.table = this;
        try
        {
            craftBook = transform.Find("CraftBook").GetComponent<CraftBook>();
            craftBook.SetCraftTable(this);
        }
        catch
        {
            Debug.LogError("无法获取craftBook", gameObject);
        }
        types = new ItemType[9];
        slots = new CraftSlot[9];
        for (int i = 0; i < SlotsParent.childCount; i++)
        {
            slots[i] = SlotsParent.GetChild(i).gameObject.GetComponent<CraftSlot>();
            types[i] = slots[i].info.type;
            slots[i].table = this;
        }
    }
    public void CheckSlots()//判断,返回值为"最多合成份数"
    {
        for (int i = 0; i < slots.Length; i++)
        {
            types[i] = slots[i].info.type;//刷新
        }

        ItemType[] types_corrected = Correct(types);
        
        int flag = 0;
        foreach (CraftInfo info in CraftManager.Instance.CraftInfos)
        {
            if (Judge( Correct(info.types), types_corrected))
            {
                outputSlot.ShowItem(info.OutPutType, info.OutPutNum);
                flag = 1;
                break;
            }
        }
        if (flag == 0)
        {
            outputSlot.CleanItem();
        }


    }
    public int GetMaxOutPutCount()
    {
        int min = 0;
        for (int i = 0; i < slots.Length; i++)
        {

            min = CompareMinNotIncludeZero(min, slots[i].info.Count);
        }
        return min;
    }
    public bool Judge(ItemType[] a, ItemType[] b)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] == b[i])
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public ItemType[] Correct(ItemType[] types)
    {
        ItemType[] types_corrected = new ItemType[9];
        int minx = 3, miny = 3;
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i] != ItemType.Nothing)
            {
                minx = CompareMin(minx, i % 3);
                miny = CompareMin(miny, i / 3);
            }
        }
        if (minx != 3)
        {
            for (int i = 0; i < types_corrected.Length; i++)
            {
                if (i + minx + 3 * miny < 9)
                    types_corrected[i] = types[i + minx + 3 * miny];
            }
        }
        return types_corrected;

    }
    private int CompareMin(int a, int b)//判断最小值
    {
        if (a < b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }
    private int CompareMinNotIncludeZero(int a, int b)
    {
        if (a == 0)
        {
            return b;
        }
        else if (b == 0)
        {
            return a;
        }
        if (a < b)
        {
            return a;
        }
        else
        {
            return b;
        }



    }
    public void CleanOnce()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].info.type != ItemType.Nothing)
            {
                slots[i].info.Count -= 1;
                slots[i].ItemUpdate();
            }
        }
    }

    public void PutEverythingBack()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].info.type != ItemType.Nothing)
            {
                int rest = InventoryManager.Instance.AddItems(slots[i].info);
                if (rest > 0)
                {
                    ItemInfo Newinfo = new ItemInfo(ItemType.Nothing,0,0);
                    Newinfo = slots[i].info.ShallowClone();
                    Newinfo.Count = rest;
                    PlayerItemManager.Instance.ThrowStuff(Newinfo);
                }
            }
            slots[i].CleanItem();
        }
        outputSlot.CleanItem();
    }
    public void PutOnVirtualImage(CraftInfo info)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowVirtualImage(ResourceSystem.Instance.GetItem(info.types[i]).AnimationSprites[0]);
        }
    }
    public void CleanVirtualImage()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].HideVirtualImage();
        }
    }
    public void PutOnItems(CraftInfo info)
    {
        
            CleanVirtualImage();
        for (int i = 0; i < slots.Length; i++)
        {
            if (info.types[i] != ItemType.Nothing)
            {
                ItemInfo Iinfo = new ItemInfo(info.types[i],1,1);
                slots[i].AddNewItem(Iinfo);
            }
        }
        CheckSlots();
        //减少该物品 
        for (int i = 0; i < info.types.Length; i++)
        {
            InventoryManager.Instance.DeleteTargetItem(info.types[i]);
        }
    }

  
}
