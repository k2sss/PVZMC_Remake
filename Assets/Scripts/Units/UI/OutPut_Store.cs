using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutPut_Store : Slot
{
    public override void Update()
    {
        if (IsEnter == true)
        {
            HasInputMouseButton = false;
            if (HasInputMouseButton == false)
            {

                TakeItemAll();
            }
        }
        AnimationSprite();
    }
    
    public void TakeItemAll()//ÄÃ×ßÎïÆ·
    {
        if (Input.GetMouseButtonDown(0) && MyItemDrag.IsEmpty())
        {
            ItemInfo t = info.ShallowClone();
            info = MyItemDrag.info.ShallowClone();
            MyItemDrag.info = t;
            HasInputMouseButton = true;
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            onClick?.Invoke();
        }
    }

}
