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
            buttonText.text = "回到原点";

        }
        else
        {
            buttonText2.text = "返回世界";
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
            SelectWindow.Instance.Show("你是否要重新开始本次关卡？",ReStart) ;
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
        SelectWindow.Instance.Show("你是否要退回到选关菜单", BackToInGameMenu);
        else
        {
            SelectWindow.Instance.Show("你是否要退回到主界面",() => SceneMgr.Instance.LoadAsync("MainTitle"));
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
        SelectWindow.Instance.Show("你是否要退出游戏",()=>Application.Quit());
    }

}
