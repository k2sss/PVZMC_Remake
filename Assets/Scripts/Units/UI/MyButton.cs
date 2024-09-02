using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string SetText;
    public OnClick onClick;
    private Text m_Text;
    private Animator animator;
    public AudioClip audioclip;
    private bool isInited;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick.Invoke();

        }
        SoundSystem.Instance.Play2Dsound(audioclip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Enter", true);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Enter", false);
    }

    private void Init()
    {
        if (isInited == false)
        {
            m_Text = transform.GetChild(0).gameObject.GetComponent<Text>();
            m_Text.text = SetText;
            animator = GetComponent<Animator>();
            isInited = true;

        }

    }
    public void UpdateText()
    {
        Init();
        m_Text.text = SetText;
    }
    [System.Serializable]
    public class OnClick : UnityEvent
    {

    }

}
