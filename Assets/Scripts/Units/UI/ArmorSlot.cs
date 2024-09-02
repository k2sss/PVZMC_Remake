using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerArmor;
public class ArmorSlot : Slot
{
    public ArmorType armortype;
    public PlayerArmManager manager;
    //禁用函数
    public override void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerArmManager>();
        base.Start();
      
    }
    public override void GetOneItemFromItemDrag()
    {
        
    }
    public override void PickUpHalfItem()
    {
      
    }
    public override void AddItemFromItemDrag()
    {
       
    }
    //更改函数
    public override void ExChangeItem()
    {
        if (Input.GetMouseButtonDown(0) && !MyItemDrag.CompareType(info)&&(
            MyItemDrag.info.type == ItemType.Nothing || CompareArmor(MyItemDrag.info.type)
            ))
        {
            ItemInfo t = info.ShallowClone();
            info = MyItemDrag.info.ShallowClone();
            MyItemDrag.info = t;
            HasInputMouseButton = true;
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            onClick?.Invoke();
            EventMgr.Instance.EventTrigger("UpdateArmor");
        }
    }
    private bool CompareArmor(ItemType type)
    {
        for (int i = 0; i < manager.armorDatas.datas.Length; i++)
        {
            for (int j = 0; j < manager.armorDatas.datas[i].groups.Length; j++)
            {
                if (manager.armorDatas.datas[i].groups[j].targetItemType == type)
                {
                    if (armortype == manager.armorDatas.datas[i].groups[j].armorType)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
        return false;
    }

}
public enum ArmorType
{
    helmet,
    chest,
    leg,
    shoe,
}