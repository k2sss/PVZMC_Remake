using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMgr : BaseManager<GameStateMgr>
{
    public bool EnterGameDirectly { get; set; }
    public string MusicName = "Fight1";
    private void Start()
    {       
           EventMgr.Instance.AddEventListener("GameStart", InputMgr.Instance.EnableAllInput);
           EventMgr.Instance.AddEventListener("GameStart", () => MusicMgr.Instance.PlayMusic(MusicName));
        if (!(EnterGameDirectly||MySystem.CanLoadLevelData()))
        {
            Invoke("StartSelectCard", 1f);
            InputMgr.Instance.UnableAllInput();
            PhoneControlMgr.Instance.UnAble = true;
            
            EnemyManager.Instance.SummonEnemy_Temple();//������ʱ����
            
            EventMgr.Instance.AddEventListener("OnRCardQuit", () => CameraAction.Instance.FollowPlayer(true));
           
            CameraAction.Instance.GoToRightSide();
        }
        //else
        //{
        //    EventMgr.Instance.EventTrigger("GameStart");
        //    MusicMgr.Instance.PlayMusic("Fight1");
        //}

    }
    public void UnableRCard()
    {
        InputMgr.Instance.UnableAllInput();
        EnemyManager.Instance.SummonEnemy_Temple();//������ʱ����
    }
    public void StartWithNoRCard()
    {  
        UIMgr.Instance.ShowUI("RSP");
        MonoController.Instance.Invoke(2,()=>EventMgr.Instance.EventTrigger("GameStart")); 
    }
    public void GameStartUnSelectCard()//��ѡ����ֱ�ӽ�����Ϸ
    {
        EventMgr.Instance.EventTrigger("OnRCardQuit");//���ÿ�Ƭ
        EventMgr.Instance.EventTrigger("GameStart");//��ʽ������Ϸ
    }
    //��Ԥѡ��Ƭ��
    public void StartSelectCard()
    {
        EventMgr.Instance.EventTrigger("StartSelectCard");//����ѡ������
    }

}
