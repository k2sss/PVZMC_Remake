using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ItemDrag : Slot
{
    public bool IsHoldMouseButton1;
    public static ItemDrag Instance;
    public override void OnPointerEnter(PointerEventData eventData)
    {

    }
    public override void OnPointerExit(PointerEventData eventData)
    {

    }
    protected override void Awake()
    {
        base.Awake();
        Instance = this;

    }
    public override void Update()
    {
        Followpointer();
        AnimationSprite();
    }
    public bool CompareType(ItemInfo oinfo)
    {
        if (oinfo.type == this.info.type)
        {
            return true;
        }
        else
        {
            return false;

        }
    }//检查是否为同一种类

    public void SubCount(int subnum)
    {
        info.Count -= subnum;
        if (info.Count < 0)
        {
            info.Count = 0;
        }
        ItemUpdate();
    }//减少数量
    public void Followpointer()
    {
        transform.position = Input.mousePosition;
    }
    public void Throw()
    {
        if (info.type != ItemType.Nothing)
        {
            PlayerItemManager.Instance.ThrowStuff(info);
            CleanItem();
            ItemUpdate();
            EventMgr.Instance.EventTrigger("OnChangeInventory");
        }
    }
    public override void OnDisable()
    {

    }
    public void OnEnable()
    {
        Instance = this;
    }

}
