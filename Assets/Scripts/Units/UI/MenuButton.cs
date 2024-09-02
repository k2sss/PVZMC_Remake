using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MenuButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Animator animator;
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("enter", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("enter", false);
    }

   
    void Start()
    {
        animator = GetComponent<Animator>();
    }

}
