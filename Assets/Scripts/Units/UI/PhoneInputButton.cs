using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneInputButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private bool IsEnter;
    private bool Pressed;
    private bool isClicked;
    private int b;
    
    private void Update()
    {
        if (isClicked == true&&b <1)
        {
            isClicked = false;
        }
        if (isClicked == true)
        {
            b--;
        }
    }
    public bool IsPressed()
    {
        return IsEnter;
    }
    public bool IsClicked()
    {
        return isClicked;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        IsEnter = true;
        isClicked = true;
        b = 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsEnter = false;
    }

}
