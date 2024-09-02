using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class R_CardSlot : MonoBehaviour
{
    private Animator animator;
    public AudioClip StartButtonSound;
    public Transform RCardParent;
    public GameObject RCard;
    private bool Clicked = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //���ɿ�Ƭ��
        for (int i = 0; i < MySystem.Instance.nowUserData.OwnedPlants.Count; i++)
        {
            GameObject c = Instantiate(RCard, RCardParent);
            c.GetComponent<R_Card>().type = MySystem.Instance.nowUserData.OwnedPlants[i];
        }
        EventMgr.Instance.AddEventListener("PickUpChest", () => gameObject.SetActive(false));

        //EnterSelectCard �¼��� �ؿ��� Startִ��

        EventMgr.Instance.AddEventListener("StartSelectCard", () => gameObject.SetActive(true));

        gameObject.SetActive(false);
    }
    public void StartButtonDown()//���¿�ʼ��ť
    {
        if (Clicked == false)
        {
            animator.SetBool("Go", true);
            MonoController.Instance.Invoke(1, () => UIMgr.Instance.ShowUI("RSP"));
            MonoController.Instance.Invoke(3, () => EventMgr.Instance.EventTrigger("GameStart"));
            SoundSystem.Instance.Play2Dsound(StartButtonSound);
            Clicked = true;
        }
    }
    public void GameObjSetActiveFalse()
    {
        EventMgr.Instance.EventTrigger("OnRCardQuit");
        gameObject.SetActive(false);
    }
}
