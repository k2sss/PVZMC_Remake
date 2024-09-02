using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitleUI : MonoBehaviour
{
    public GameObject SelectSaveUIObj;
    public MyButton[] childButtons;
    public SelectSaveUI Uiscr;
    private void Start()
    {
        childButtons = new MyButton[transform.childCount];
        for (int i = 0; i < childButtons.Length; i++)
        {
            childButtons[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<MyButton>();
        }
        DefaultSet();
    }
    private void DefaultSet()
    {
        CleanSet();
        childButtons[0].SetText = "开始游戏";
        childButtons[0].onClick.AddListener(StartGame);
        childButtons[1].SetText = "游戏设置";
        childButtons[1].onClick.AddListener(GameSetting);
        childButtons[2].SetText = "关于作者";
        childButtons[2].onClick.AddListener(DeveloperInfomation);
        childButtons[3].SetText = "退出游戏";
        childButtons[3].onClick.AddListener(ExitGame);
        UpdateText();
    }
    private void CleanSet()
    {
        for(int i =0;i<childButtons.Length;i++)
        {
            childButtons[i].onClick.RemoveAllListeners();
        }
        for(int i = 0;i<childButtons.Length;i++)
        {
            childButtons[i].SetText = "";
           
        }
    }
    private void UpdateText()
    {
        for (int i = 0; i < childButtons.Length; i++)
        {
            
            childButtons[i].UpdateText();

        }
    }

    public void StartGame()
    {
        // SceneMgr.Instance.LoadAsync("InGameMenum");
        SelectSaveUIObj.SetActive(true);
        Uiscr.SetAnimation2Origin();
        CleanSet();
        childButtons[0].SetText = "返回";
        childButtons[0].onClick.AddListener(DefaultSet);
        childButtons[0].onClick.AddListener(()=>Uiscr.Close());
        UpdateText();
    }
    public void GameSetting()
    {
        UIMgr.Instance.PushUIByKey("Setting");
    }
    public void DeveloperInfomation()
    {
        Application.OpenURL("https://space.bilibili.com/22364390");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
