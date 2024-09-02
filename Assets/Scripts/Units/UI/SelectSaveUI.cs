using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SelectSaveUI : MonoBehaviour
{
    public GameObject[] saveButtons;
    
    void Start()
    {
        gameObject.SetActive(false); 
        UpdateButton();
    }

    public void SetAnimation2Origin()
    {
        gameObject.GetComponent<Animator>().SetBool("Quit", false);
    }
    public void Close()
    {

        gameObject.GetComponent<Animator>().SetBool("Quit", true);
    }
    public void Disappear()
    {
        gameObject.SetActive(false);
    }
    public void CancleALLButtonSelect()
    {
        for(int i =0;i<saveButtons.Length;i++)
        {
            saveButtons[i].GetComponent<DataButton>().CancleSelect();

        }

    }
    public void CreateSave1()
    {
        InputWindow.Instance.Show("请输入你的用户名？", () => CreateNewSave(0));
    }
    public void CreateSave2()
    {
        InputWindow.Instance.Show("请输入你的用户名？", () => CreateNewSave(1));
    }
    public void CreateSave3()
    {
        InputWindow.Instance.Show("请输入你的用户名？", () => CreateNewSave(2));
    }
    public void DeleteN(int index)
    {
        MySystem.Instance.DeleteUser(index);
        UpdateButton();

    }
    public void Delete1()
    {
          SelectWindow.Instance.Show("你是否要删除该存档", () => DeleteN(0));
    }
    public void Delete2()
    {
        SelectWindow.Instance.Show("你是否要删除该存档", () => DeleteN(1));

    }
    public void Delete3()
    {
        SelectWindow.Instance.Show("你是否要删除该存档", () => DeleteN(2));

    }
    private void LoadGame(int index)
    {
        MySystem.Instance.nowUserInfo.index = index;
        MySystem.Instance.SaveNowInfo();
        string Scene = MySystem.getUserData().NowInGameMenuSceneName;
        Debug.Log(Scene);
        if(Scene != null&&Scene != "")
        {
           
            SceneMgr.Instance.LoadAsync(Scene);
        }
        else
        {

           SceneMgr.Instance.LoadAsync("InGameMenum");
        }
    }
    public void Load1()
    {
        LoadGame(0);
    }
    public void Load2()
    {
        LoadGame(1);
    }
    public void Load3()
    {
        LoadGame(2);
    }
    public void CreateNewSave(int index)
    {
        if(InputWindow.Instance.GetText().Length > 8)
        {
            InfoWindow.Instance.Show("错误", "输入的ID过长");
            return;
        }


        if(InputWindow.Instance.GetText()!="")
        {
            MySystem.Instance.CreateNewUser(InputWindow.Instance.GetText(),index);
            UserData newData = new UserData();
            newData.FirstGame = true;
            newData.Username = InputWindow.Instance.GetText();
            MySystem.Instance.nowUserInfo.index = index;
            SaveSystem.SaveUserData("data", "", newData);
            UpdateButton();
        }
        else
        {
            InfoWindow.Instance.Show("错误", "你未输入任何ID");
        }
       
    }
    public void UpdateButton()//刷新
    {
        MySystem.Instance.LoadAllUserInfo();
        for (int i = 0; i < 3; i++)
        {
            saveButtons[i].SetActive(false);
        }
        for (int i =0;i<MySystem.Instance.userList.users.Count;i++)
        {
            saveButtons[MySystem.Instance.userList.users[i].index].SetActive(true);
            saveButtons[MySystem.Instance.userList.users[i].index].transform.Find("name")
                .GetComponent<Text>().text = MySystem.Instance.userList.users[i].UserName;
            DateTime now = MySystem.Instance.userList.users[i].LastSaveTime.ToDateTime();
            string formattedTime = now.ToString("M-d HH:mm");
            saveButtons[MySystem.Instance.userList.users[i].index].transform.Find("time")
               .GetComponent<Text>().text = formattedTime;


        }

    }
}
