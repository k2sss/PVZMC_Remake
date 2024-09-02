using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : Slot
{

    public override void Update()
    {
        if (IsEnter == true)
        {
            HasInputMouseButton = false;
            if (HasInputMouseButton == false)
                ExChangeItem();
            if (HasInputMouseButton == false)
                AddItemFromItemDrag();
            if (HasInputMouseButton == false)
                GetOneItemFromItemDrag();
            if (HasInputMouseButton == false)
                PickUpHalfItem();
            if (HasInputMouseButton == true)
            {
                EventMgr.Instance.EventTrigger("ReInventory");
                EventMgr.Instance.EventTrigger("OnChangeInventory");
            }
        }
        AnimationSprite();
    }
}
