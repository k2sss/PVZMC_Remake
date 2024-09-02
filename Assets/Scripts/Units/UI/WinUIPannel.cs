using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerArmor;

public class WinUIPannel : Pannel
{
    private void Start()
    {
        EventMgr.Instance.AddEventListener("GameWin", SaveTheDataWhenGameWin);
    }
    public override void Init()
    {
        base.Init();
        Invoke("EventOn", 0.01f);
        gameObject.SetActive(false);
    }
    public void EventOn()
    {
        EventMgr.Instance.AddEventListener("OnOpenChest", () => UIMgr.Instance.PushUIByKey("WinUIChest"));
        EventMgr.Instance.AddEventListener("OnOpenChest", () => transform.Find("Chest").GetComponent<Animator>().SetBool("Go", true));
    }
    public void BackButton()
    {
      SelectWindow.Instance.Show("你是否要返回到主世界？", BackToInGameMenu);
    }
    public void BackToInGameMenu()
    {
        SaveTheDataWhenGameWin();
        string menuName = MySystem.Instance.nowUserData.NowInGameMenuSceneName;
        if(menuName != null && menuName != "")
        SceneMgr.Instance.LoadAsync(menuName);
        else
        {
        SceneMgr.Instance.LoadAsync("InGameMenum");
        }
    }
    public void SaveTheDataWhenGameWin()
    {

        bool flag = false;
        //搜寻
        for (int i = 0; i < MySystem.Instance.nowUserData._levelFinishData.Count; i++)
        {
            if(MySystem.Instance.nowUserData.levelName == MySystem.Instance.nowUserData._levelFinishData[i].LevelName)
            {
                flag = true;//本关已储存
                switch(MySystem.Instance.nowUserData.LevelType)
                {
                    case degreetype.normal:
                        MySystem.Instance.nowUserData._levelFinishData[i].finishTheNormalLevel = true;
                        break;
                    case degreetype.hard:
                        MySystem.Instance.nowUserData._levelFinishData[i].finishTheHardLevel = true;
                        break;
                    case degreetype.hell:
                        MySystem.Instance.nowUserData._levelFinishData[i].finishTheHelllLevel = true;
                        break;
                }
                break;
                
            }
        }
        if(flag == false)//
        {

            LevelFinishData newdata = new LevelFinishData();
            newdata.LevelName = MySystem.Instance.nowUserData.levelName;
            switch (MySystem.Instance.nowUserData.LevelType)
            {
                case degreetype.normal:
                    newdata.finishTheNormalLevel = true;
                    break;
                case degreetype.hard:
                    newdata.finishTheHardLevel = true;
                    break;
                case degreetype.hell:
                    newdata.finishTheHelllLevel = true;
                    break;
            }

            MySystem.Instance.nowUserData._levelFinishData.Add(newdata);
        }

        MySystem.Instance.nowUserData.levelName = "";
        MySystem.Instance.nowUserData.sceneName = "";
        MySystem.Instance.nowUserData.items = InventoryManager.Instance.InventoryItems;
        PlayerHealth.instance.gameObject.GetComponent<PlayerArmManager>().Save();
        
        MySystem.Instance.SaveNowUserData();


    }
}
