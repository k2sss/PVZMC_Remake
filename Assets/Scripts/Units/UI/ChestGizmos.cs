using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ChestGizmos : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    private Outline outline;
    private Image image;
    private Animator animator;
    private bool IsSelected;
    private void Start()
    {
        gameObject.SetActive(false);
        EventMgr.Instance.AddEventListener("GameWin", () => gameObject.SetActive(true));
        outline = GetComponent<Outline>();
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(IsSelected == false)
        outline.effectDistance = new Vector2(2.3f, -2.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected == false)
            outline.effectDistance = new Vector2(0f, 0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsSelected == false)
        {
            EventMgr.Instance.EventTrigger("PickUpChest");
        }
        
        IsSelected = true;
        animator.SetBool("Go", true);
        animator.SetBool("Go2", true);
    }
    public void OnOpenChest()
    {
        SoundSystem.Instance.Play2Dsound("Chest");
        EventMgr.Instance.EventTrigger("OnOpenChest");
    }
  
}
