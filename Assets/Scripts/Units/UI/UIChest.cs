using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChest : MonoBehaviour
{
    private ChestSlot slots;
    private void Start()
    {
       // EventMgr.Instance.AddEventListener("OnOpenChest", ()=>InventoryManager.Instance.OpenBag(InventoryUIType.chest)) ;
        slots = transform.Find("ChestSlots").GetComponent<ChestSlot>();
    }
    public void TakeItemAll()
    {
        for (int i = 0; i < slots.slots.Length; i++)
        {
            int rest = InventoryManager.Instance.AddItems(slots.slots[i].info);
            if (rest == 0)
            {
                slots.slots[i].CleanItem();
            }
            else
            {
                slots.slots[i].info.Count = rest;
            }
        }
    }
    

}
