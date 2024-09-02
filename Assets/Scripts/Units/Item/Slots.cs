using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{
    private Slot[] slots = new Slot[36];

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = transform.GetChild(i).gameObject.GetComponent<Slot>();
        }
        MonoController.Instance.InvokeUnScaled(0.01f, UpdateItem);
        EventMgr.Instance.AddEventListener("OnChangeInventory", UpdateItem);
        EventMgr.Instance.AddEventListener("ReInventory", ReInventory);
    }
    public void UpdateItem()//��Inventory iteminfos���ݵ��뵽Slot,��ˢ��
    {
        ItemInfo[] infos = InventoryManager.Instance.GetItemInfos();
        for (int i = 0; i < infos.Length; i++)
        {
            slots[i].info = infos[i];
        }
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ItemUpdate();
        }
    }
    public void ReInventory()//��Slot�����ݵ��뵽Inventory iteminfos
    {
        if (InventoryManager.Instance.targetSlots == this)
        {
            ItemInfo[] infos = InventoryManager.Instance.GetItemInfos();
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i] = slots[i].info;
            }
        }

    }
    public void OnEnable()
    {
        InventoryManager.Instance.targetSlots = this;
    }

}
