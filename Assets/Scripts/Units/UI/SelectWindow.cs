using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SelectWindow : BaseManager<SelectWindow>
{

    public Button buttonleft;//һ��Ϊȡ����
    public Button buttonRight;//һ��Ϊȷ�ϼ�
    public GameObject target;
    public Text contentText;
    private void Start()
    {
        target.SetActive(false);
    }
    public void Show(string content, UnityAction action)
    {
        target.SetActive(true);
        gameObject.GetComponent<Animator>().SetBool("Quit",false);
        //buttonleft.onClick.RemoveAllListeners();
        buttonRight.onClick.RemoveAllListeners();
  
        buttonRight.onClick.AddListener(action);
  
        buttonRight.onClick.AddListener(Cancel);

        contentText.text = content;
    } 
    public void Cancel()
    {
        gameObject.GetComponent<Animator>().SetBool("Quit", true);
        SoundSystem.Instance.Play2Dsound("Click");
    }
    public void Disapear()
    {
        target.SetActive(false);
    }
}
