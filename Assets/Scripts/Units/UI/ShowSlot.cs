using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowSlot : Slot
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
       
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
       
    }
    public override void Start()
    {
        ItemUpdate();
    }
    public override void OnDisable()
    {
        
    }
}
