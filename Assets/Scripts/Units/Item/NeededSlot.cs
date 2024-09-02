using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NeededSlot : Slot
{
    public bool IsReady;
    public override void OnPointerEnter(PointerEventData eventData)
    {

    }
    public override void OnPointerExit(PointerEventData eventData)
    {

    }
    public override void OnDisable()
    {

    }
    public override void ReFreshCountText()
    {
        
        InventoryManager.Instance.UpdateItemDic();
        if (info.type == ItemType.Nothing)
        {
            CountText.text = "";
        }
        else
        {
            CountText.text = InventoryManager.Instance.GetItemCount(info.type).ToString() + "/" + info.Count.ToString();
        }


        if (InventoryManager.Instance.GetItemCount(info.type) < info.Count)
        {   IsReady = false;
            CountText.color = Color.red;
        }
        else
        {
            IsReady = true;
            CountText.color = Color.white;
        }
    }
}
