using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OverWorld;
public class MenuMgr:Pannel
{
    public Text buttonText;
    public Text buttonText2;
    public override void Init()
    {
        base.Init();
        UIMgr.Instance.AddPushAction(PannelName, () => Time.timeScale = 0);
        UIMgr.Instance.AddPushAction(PannelName, () => InputMgr.Instance.UnableAllInput());
        UIMgr.Instance.AddPopAction(PannelName, () => Time.timeScale = 1);
        UIMgr.Instance.AddPopAction(PannelName, () => InputMgr.Instance.EnableAllInput());
        gameObject.SetActive(false);
        if (!MySystem.IsInLevel())
        {
            buttonText.text = "�ص�ԭ��";

        }
        else
        {
            buttonText2.text = "��������";
        }
       
    }

    public void GameSet()
    {
        UIMgr.Instance.PushUIByKey("Setting");
    }
    public void BackGame()
    {
        UIMgr.Instance.PopUI();
    }
    public void ReStartGame()
    {
        if (MySystem.IsInLevel())
            SelectWindow.Instance.Show("���Ƿ�Ҫ���¿�ʼ���ιؿ���",ReStart) ;
        else
        {
            if(OverWorldMgr.Instance!=null)
            GameObject.FindGameObjectWithTag("Player").transform.position = OverWorldMgr.Instance.RestartPos;
            //BackGame();
        }

    }
    private void ReStart()
    {
        MySystem.Instance.nowLeveldata.canLoad = false;
        MySystem.Instance.SaveNowLevelData();
        SceneMgr.Instance.LoadAsync(SceneMgr.Instance.GetSceneName());
    }
    public void ExitToMainWorld()
    {
        if(MySystem.IsInLevel())
        SelectWindow.Instance.Show("���Ƿ�Ҫ�˻ص�ѡ�ز˵�", BackToInGameMenu);
        else
        {
            SelectWindow.Instance.Show("���Ƿ�Ҫ�˻ص�������",() => SceneMgr.Instance.LoadAsync("MainTitle"));
            MySystem.Instance.SaveNowUserData();
        }
        
    }
    private void BackToInGameMenu()
    {
        MySystem.Instance.nowLeveldata.canLoad = false;
        MySystem.Instance.SaveNowLevelData();
        string OverWorldName = MySystem.Instance.nowUserData.NowInGameMenuSceneName;
        if (OverWorldName != null &&OverWorldName != "")
        {
            SceneMgr.Instance.LoadAsync(OverWorldName);
        }
        else
        {
            SceneMgr.Instance.LoadAsync("InGameMenum");
        }
        
    }
    public void ExitGame()
    {
      
        if (!MySystem.IsInLevel())
        {
            MySystem.Instance.SaveNowUserData();
        } 
        SelectWindow.Instance.Show("���Ƿ�Ҫ�˳���Ϸ",()=>Application.Quit());
    }

}
