using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MySystem :BaseManager<MySystem>
{
    public UserInfo nowUserInfo;
    public UserInfoList userList;
    public UserData nowUserData;
    public UnityAction whenSaveAction { get; set; }
    public LevelData nowLeveldata;
    
    
    [ContextMenu("打开存档文件夹")]
    public void OpenSaveFile()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
    private void Start()
    {
        Time.timeScale = 1;
        if(InventoryManager.Instance != null)
        InventoryManager.Instance.Init();
        if (ItemSlotsManager.Instance != null)
        {
            ItemSlotsManager.Instance.Init();
        }
        //Debug.Log(SceneManager.GetActiveScene().name);
        //MonoController.Instance.Invoke(3, () => SelectWindow.Instance.Show("111", () => Debug.Log("111")));
    }

    protected override void Awake()
    {
        base.Awake();
        //读取
        LoadAllUserInfo();

        UserInfo userinfo = SaveSystem.Load<UserInfo>("NowUserInfo","SystemData");
        if(userinfo != null)
        {
            nowUserInfo = userinfo;
        }
        UserData data = SaveSystem.LoadUserData<UserData>("data", "");
        
        if (data != null)
        {   
            nowUserData = data;
            if (nowUserData.CardSlotCount == 0)
            {
                nowUserData.CardSlotCount = 6;
            }
        }
        LevelData ldata = SaveSystem.LoadUserData<LevelData>("levelData", "");
        if (ldata != null)
        {
            nowLeveldata = ldata;
        }

    }
    public static UserData getUserData()
    {
       

        UserData data = SaveSystem.LoadUserData<UserData>("data", "");
       
        if (data != null)
        {
            
            if (data.CardSlotCount == 0)
            {
                data.CardSlotCount = 6;
            }
            
        }
        return data;
    }
    public static bool CanLoadLevelData()
    {
        LevelData data = SaveSystem.LoadUserData<LevelData>("levelData", "");
        UserData udata = SaveSystem.LoadUserData<UserData>("data", "");
        if(data != null)
        if (data.LevelName == udata.levelName && data.LevelType == udata.LevelType && data.canLoad == true)
        {
            return true;
        }
        return false;
    }
    public void SaveNowUserData()
    {
       
        whenSaveAction?.Invoke();
       
        SaveSystem.SaveUserData("data", "", nowUserData);
        nowUserInfo.LastSaveTime = DateTimeSerlizable.Now();
        SaveNowInfo();
        userList.users[nowUserInfo.index].LastSaveTime = DateTimeSerlizable.Now();
        SaveAllUserInfo();
        
    }
    public void SaveNowLevelData()
    {
        SaveSystem.SaveUserData("levelData", "", nowLeveldata);
        nowUserInfo.LastSaveTime = DateTimeSerlizable.Now();
        SaveNowInfo();
        userList.users[nowUserInfo.index].LastSaveTime = DateTimeSerlizable.Now();
        SaveAllUserInfo();
    }
    public void SaveNowInfo()
    {
        SaveSystem.Save("NowUserInfo", "SystemData", nowUserInfo);
    }
    public void SaveAllUserInfo()//保存所有User的数据
    {
        SaveSystem.Save("userlist", "SystemData", userList);

    }
    public void LoadAllUserInfo()//读取所有User的数据
    {
        UserInfoList list = new UserInfoList();

        list = SaveSystem.Load<UserInfoList>("userlist", "SystemData");
        if (list != null)
        {
            userList = list;
        }
    }
    public void CreateNewUser(string name,int index)//创建一个新的User
    {
        UserInfo newuserinfo = new UserInfo();
        newuserinfo.UserName = name;
        newuserinfo.index = index;
        newuserinfo.CreateTime = DateTimeSerlizable.Now();
        newuserinfo.LastSaveTime = DateTimeSerlizable.Now();
        userList.users.Add(newuserinfo);
        SaveAllUserInfo();
    }
    public void DeleteUser(int index)//删除一个指定的User
    {
        foreach (UserInfo userinfo in userList.users)
        {
            if (userinfo.index == index)
            {
                userList.users.Remove(userinfo);
                break;
            }
        }
        SaveAllUserInfo();
        SaveSystem.DeleteUserData(index);
    }

    public static bool IsInLevel()
    {
        InGameMenuInfos infos = FileLoadSystem.ResourcesLoad<InGameMenuInfos>("SciptableObjects/InGameMenuInfo");
        string nowSceneName = SceneManager.GetActiveScene().name;
        for (int i = 0; i < infos.list.Count; i++)
        {
            if (infos.list[i].SceneName == nowSceneName)
            {
                return false;
            }
        }
        return true;
    }
}
[System.Serializable]
public class UserInfoList
{
  public  List<UserInfo> users = new List<UserInfo>();
}
[System.Serializable]
public class UserInfo
{    
    public string UserName;
    public int index;
    public DateTimeSerlizable CreateTime;
    public DateTimeSerlizable LastSaveTime;
}
[System.Serializable]
public class DateTimeSerlizable
{
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;
    public int Second;
    public DateTimeSerlizable(int year,int Month,int Day,int Hour,int Minute,int Second)
    {

        Year = year;
        this.Month = Month;
        this.Day = Day;
        this.Hour = Hour;
        this.Minute = Minute;
        this.Second = Second;
    }
    public static DateTimeSerlizable Now()
    {
        return new DateTimeSerlizable(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
    }
    public DateTime ToDateTime()
    {

        return new DateTime(Year, Month, Day, Hour, Minute, Second);
    }
}