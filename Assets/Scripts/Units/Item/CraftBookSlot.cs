using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftBookSlot : Slot
{
    public int ID;
    public GameObject Black;
    public bool IsAble;
    public CraftInfo C_info;
    public CraftBook book;
    public override void Start()
    {
        base.Start();
        Black = transform.Find("Black").gameObject;
        if (IsAble == true)
        {
            Black.SetActive(false);
        }
        else
        {
            Black.SetActive(true);
        }
    }
    public override void Update()
    {
       
        if (IsEnter == true)
        {
            ClickToPutItemsOnCraftTable();
        }
        AnimationSprite();

    }
    private void ClickToPutItemsOnCraftTable()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundSystem.Instance.Play2Dsound("Click");

            if (IsAble == true)
            {
                
                if (book.SelectCraftBookSlotID != ID)
                {   
                    book.craftTable.PutEverythingBack();
                    book.SelectCraftBookSlotID = ID;
                    ShowVirtualImage();
                }
                else
                {
                    PutItems();
                }
            }
            else
            {
                book.craftTable.PutEverythingBack();
                book.SelectCraftBookSlotID = ID;
                ShowVirtualImage(); 
            }
            
        }
    }
    private void ShowVirtualImage()
    {
        book.craftTable.PutOnVirtualImage(C_info);
    }
    private void PutItems()
    {
        book.craftTable.PutOnItems(C_info);
        EventMgr.Instance.EventTrigger("OnChangeInventory");
    }



}
