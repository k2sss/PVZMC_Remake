using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeededSlotMgr : MonoBehaviour
{
    public NeededSlot[] slots;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
       slots = new NeededSlot[transform.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
         slots[i] = gameObject.transform.GetChild(i).GetComponent<NeededSlot>();
        }
    }
    public void ItemUpdate()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ItemUpdate();
        }

    }
    public void Give(ItemInfo[] items)
    {
       
        int n = Mathf.Min(items.Length, slots.Length);
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].CleanItem();
        }
        
        for(int i = 0; i < n; i++)
        {
            slots[i].info = items[i].ShallowClone();
        }
        ItemUpdate();
    }
    public bool CanUnLock()
    {
        for(int i = 0; i < slots.Length;i++)
        {
            if (slots[i].IsReady == false&& slots[i].info.type != ItemType.Nothing)
            {
                return false;
            }
        }
        return true;
    }
}
