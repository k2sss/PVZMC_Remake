using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemSlots : Slot
{
    public GameObject SelectSquare;
    public override void Start()
    {
        base.Start();
    }
    public void Selected()
    {
        SelectSquare.SetActive(true);
    }
    public void CanCleSelected()
    {
        SelectSquare.SetActive(false);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {

    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }
    public override void OnDisable()
    {
        
    }
}
