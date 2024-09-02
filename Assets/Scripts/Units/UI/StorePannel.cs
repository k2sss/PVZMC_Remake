using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePannel : Pannel
{
    public List<StoreItem> storeItem = new List<StoreItem>();
    public Transform contentParent;
    public GameObject StoreSlot;
    public Text OutPutItemNameText;
    public ShowSlot[] showSlots;
    public Slot[] paySlots;
    private string target;
    private void Start()
    {
        MySystem.Instance.nowUserData.AddstoreItem("木棍");
        MySystem.Instance.nowUserData.AddstoreItem("工作台");
        MySystem.Instance.nowUserData.AddstoreItem("绿宝石1");
        MySystem.Instance.nowUserData.AddstoreItem("萨卡班甲鱼");
        MySystem.Instance.nowUserData.AddstoreItem("生可乐");
        MySystem.Instance.nowUserData.AddstoreItem("蜜汁鸡腿");
        MySystem.Instance.nowUserData.AddstoreItem("绿宝石2");
        MySystem.Instance.SaveNowUserData();

        InitSlot();
        
    }
    public StoreItem GetStoreItem(string ID)
    {
        for (int i = 0; i < storeItem.Count; i++)
        {
            if (storeItem[i].ID == ID)
            {
                if (storeItem[i].needItems.Length == 1)
                {
                    ItemInfo temp = storeItem[i].needItems[0].ShallowClone();
                    storeItem[i].needItems = new ItemInfo[2];
                    storeItem[i].needItems[0] = temp.ShallowClone();
                    storeItem[i].needItems[1] = new ItemInfo(ItemType.Nothing,0,0);
                }

                return storeItem[i];
            }
        }
        return null;

    }
    public void InitSlot()//初始化创建
    {

        for (int i = 0; i < MySystem.Instance.nowUserData.storeItemsID.Count; i++)
        {
            GameObject newSlot = Instantiate(StoreSlot, contentParent);

            newSlot.GetComponent<Slot>().info = GetStoreItem(MySystem.Instance.nowUserData.storeItemsID[i]).outPutItem;

            newSlot.GetComponent<Slot>().ItemUpdate();

            string newId = MySystem.Instance.nowUserData.storeItemsID[i];
            newSlot.GetComponent<Button>().onClick.AddListener(() => OnSlotCliked(newId));
        }

    }
    public void OnSlotCliked(string ID)
    {
        target = ID;
        OutPutItemNameText.text = ResourceSystem.Instance.GetItem(GetStoreItem(ID).outPutItem).myName;
        
        showSlots[1].info.type = ItemType.Nothing;
        showSlots[1].ItemUpdate();

        for (int j = 0; j < 2 && j < GetStoreItem(ID).needItems.Length; j++)
        {
            showSlots[j].info = GetStoreItem(ID).needItems[j].ShallowClone();
            showSlots[j].ItemUpdate();
        }
        showSlots[2].info = GetStoreItem(ID).outPutItem;
        showSlots[2].ItemUpdate();
    }
    public void Pay()
    {
        SoundSystem.Instance.Play2Dsound("Click");
        if (target != null && target != "")
        {

            StoreItem item = GetStoreItem(target);
            int flag = 0;
            for (int i = 0; i < 2; i++)
            {
                if (paySlots[i].info.type == item.needItems[i].type)
                    if (paySlots[i].info.Count >= item.needItems[i].Count)
                    {
                        flag++;
                    }
            }
            if (flag == 2)
            {


                if (paySlots[2].info.type == ItemType.Nothing)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        paySlots[i].info.Count -= item.needItems[i].Count;
                        paySlots[i].ItemUpdate();
                    }
                    paySlots[2].info = item.outPutItem.ShallowClone();
                    paySlots[2].ItemUpdate();
                }
                else if (paySlots[2].info.type == item.outPutItem.type 
                    && paySlots[2].info.Count + item.outPutItem.Count<=ResourceSystem.Instance.GetItem(item.outPutItem.type).MaxAmount)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        paySlots[i].info.Count -= item.needItems[i].Count;
                        paySlots[i].ItemUpdate();
                    }
                    paySlots[2].info.Count += item.outPutItem.Count;
                    paySlots[2].ItemUpdate();
                }

            }
        }


    }

    public void BackToMainWorld()
    {
        for (int i = 0; i < 3; i++)
        {InventoryManager.Instance.AddItems(paySlots[i].info);
            paySlots[i].info.type = ItemType.Nothing;
            paySlots[i].info.Count = 0;
            paySlots[i].ItemUpdate();
        }
        


        SceneMgr.Instance.LoadAsync("InGameMenum");
        //保存
        MySystem.Instance.SaveNowUserData();
    }
}

[System.Serializable]
public class StoreItem
{
    public string ID;
    public ItemInfo[] needItems;
    public ItemInfo outPutItem;

    public StoreItem(string iD, ItemInfo[] needItems, ItemInfo outPutItem)
    {
        ID = iD;
        this.needItems = needItems;
        this.outPutItem = outPutItem;
    }
}
