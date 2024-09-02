using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotsManager : BaseManager<ItemSlotsManager>
{
    public ItemInfo[] iteminfos;
    public Button[] buttons;
    public ItemSlots[] Islots;
    public int SelectCount = 0;
    public void Init()
    {
        InitSlot();
        UpdateItemSlotsSelectSquare();
        EventMgr.Instance.AddEventListener("OnChangeInventory", UpdateSlots);
        UpdateSlots();
    }
    private void Update()
    {
        InputSelectCount();
    }
    private void InitSlot()
    {
        Islots = new ItemSlots[transform.childCount];
        buttons = new Button[transform.childCount];
        iteminfos = InventoryManager.Instance.GetItemInfos();
        for (int i = 0; i < transform.childCount; i++)
        {
            ItemSlots itemslot = transform.GetChild(i).gameObject.GetComponent<ItemSlots>();
            Islots[i] = itemslot;
            buttons[i] = transform.GetChild(i).gameObject.GetComponent<Button>();
            int newI = i;
            buttons[i].onClick.AddListener(() => Switch(newI));
        }
    }
    public void Switch(int n)
    {
        SelectCount = n;
        UpdateItemSlotsSelectSquare();
        PlayerItemManager.Instance.ChangeType();
    }
    public void UpdateSlots()
    {
        for (int i = 0; i < Islots.Length; i++)
        {
            Islots[i].info = iteminfos[i];
            Islots[i].ItemUpdate();
        }

    }
    public void UpdateItemSlotsSelectSquare()
    {
        for (int i = 0; i < Islots.Length; i++)
        {
            if (i == SelectCount)
            {
                Islots[i].Selected();
            }
            else
            {
                Islots[i].CanCleSelected();
            }
        }
        PlayerItemManager.Instance.OnSwitchItem();
    }
    public void InputSelectCount()
    {

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            SelectCount -= 1;
            UpdateItemSlotsSelectSquare();
            PlayerItemManager.Instance.ChangeType();
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            SelectCount += 1;
            UpdateItemSlotsSelectSquare();
            PlayerItemManager.Instance.ChangeType();
        }

        if (SelectCount > transform.childCount - 1)
        {
            SelectCount = 0;
            UpdateItemSlotsSelectSquare();
            PlayerItemManager.Instance.ChangeType();
        }
        if (SelectCount < 0)
        {
            SelectCount = transform.childCount - 1;
            UpdateItemSlotsSelectSquare();
            PlayerItemManager.Instance.ChangeType();
        }
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1+i))
            {
                SelectCount = i;
                UpdateItemSlotsSelectSquare();
                PlayerItemManager.Instance.ChangeType();
            }

        }


    }
    public Sprite GetNowSprite()
    {
        Sprite go = ResourceSystem.Instance.GetItem(iteminfos[SelectCount]).AnimationSprites[0];
        return go;
    }
    public ItemType GetnowType()
    {
        ItemType go = iteminfos[SelectCount].type;
        return go;
    }
    public void SubCount()
    {
        iteminfos[SelectCount].Count -= 1;
        EventMgr.Instance.EventTrigger("OnChangeInventory");
        PlayerItemManager.Instance.ChangeType();
    }
    public void SubDurability()
    {
        if (ResourceSystem.Instance.GetItem(iteminfos[SelectCount].type).MaxDurability == 1)
            return;

        iteminfos[SelectCount].Durability -= 1;
        EventMgr.Instance.EventTrigger("OnChangeInventory");
        PlayerItemManager.Instance.ChangeType();
    }
    public int GetMaxDurability()
    {
        return ResourceSystem.Instance.GetItem(iteminfos[SelectCount]).MaxDurability;
    }

}
