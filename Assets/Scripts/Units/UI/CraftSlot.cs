using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CraftSlot : Slot
{
    public CraftTable table;
    private Image VirtualImage;


    public override void Start()
    {
        base.Start();
        VirtualImageInit();
        onClick += OnClick;
    }
    public void OnClick()
    {
        table.CheckSlots();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PlayerItemManager.Instance.ThrowStuff(info);
        CleanItem();
        ItemUpdate();
    }
    private void VirtualImageInit()
    {
        GameObject item = transform.Find("Item").gameObject;
        GameObject newI = Instantiate(item, transform);
        newI.transform.position = item.transform.position;
        VirtualImage = newI.GetComponent<Image>();
        VirtualImage.raycastTarget = false;
        HideVirtualImage();
    }
    public void ShowVirtualImage(Sprite sprite)
    {
        VirtualImage.color = new Color(1, 1, 1, 0.4f);
        VirtualImage.sprite = sprite;

    }
    public void HideVirtualImage()
    {
        VirtualImage.color = new Color(1, 1, 1, 0);
    }
}
