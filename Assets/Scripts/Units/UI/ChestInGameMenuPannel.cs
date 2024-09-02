using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInGameMenuPannel : Pannel
{
    public Transform slotTransfrom;
    private Slot[] targetSlots;
    public int currentID;
    private void Awake()
    {
         InitSlot();
    }
    private void Start()
    {
       
        UIMgr.Instance.AddPopAction("Chest_InGameMenu", Save);
    }
    public void InitSlot()
    {
        targetSlots = new Slot[slotTransfrom.childCount];
        for (int i = 0; i < slotTransfrom.childCount; i++)
        {
            targetSlots[i] = slotTransfrom.GetChild(i).GetComponent<Slot>();
        }
    }
    private void CleanItem()
    {
        for (int i = 0; i < targetSlots.Length; i++)
        {
            targetSlots[i].info = new ItemInfo(ItemType.Nothing, 0, 0);
            targetSlots[i].ItemUpdate();
        }
    }
    public void GetSavedData(ItemInfo[] chestinfos)
    {
        CleanItem();
        if (chestinfos.Length != 0)
        {
            for (int i =0;i<targetSlots.Length;i++)
            {
                targetSlots[i].info = chestinfos[i].ShallowClone();
                targetSlots[i].ItemUpdate();
            }
        }
    }
    //±£´æ
    public void SaveData(ref ItemInfo[] chestinfos)
    {
        chestinfos = new ItemInfo[27];
        for (int i = 0; i < targetSlots.Length; i++)
        {
            chestinfos[i]=targetSlots[i].info;
        }
        MySystem.Instance.SaveNowUserData();
    }
    private void Save()
    {
        switch (currentID)
        {
            
            case 0:
                SaveData(ref MySystem.Instance.nowUserData.items_inChest);
                break;
            case 1:
                SaveData(ref MySystem.Instance.nowUserData.items_inChest1);
                break;
        }
      
    }
}
