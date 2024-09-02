using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class UserData
{
    public string NowInGameMenuSceneName;
    public bool FirstGame;
    public string Username;
    //游戏中记录
    public string levelName;
    public string sceneName;
    public degreetype LevelType;
    public int CardSlotCount;
    public float PlayerMaxHealth;
    //背包物品
    public ItemInfo[] items;
    public ItemInfo[] armorItems;//护甲栏上的物品
    //主界面箱子储存的物品
    public ItemInfo[] items_inChest;
    public ItemInfo[] items_inChest1;
    //关卡完成程度
    public List<LevelFinishData> _levelFinishData = new List<LevelFinishData>();
    //现在拥有的植物
    public List<PlantsType> OwnedPlants = new List<PlantsType>();
    //
    public List<int> UnLockedNodes = new List<int>();
    //商店物品
    public List<string> storeItemsID = new List<string>();
    //世界存档
    public OverWorldInfo overWorldInfo;
    public List<OverWorldInfo> overWorldInfos = new List<OverWorldInfo>();

    public bool UnLockCraftTable;//解锁工作台
    public bool UnLockChest;//解锁箱子
    public bool UnLockChest1;//解锁箱子1
    public bool UnLockStore;//解锁商店
    public bool AddPlantInUserData(PlantsType type)
    {
        for(int i = 0;i< OwnedPlants.Count; i++)
        {
            if (OwnedPlants[i] == type)
            {
                return false;
            }

        }
        OwnedPlants.Add(type);
        return true;
    }
    public bool AddLockedNodes(int NodeID)
    {
        for(int i =0;i<UnLockedNodes.Count;i++)
        {
            if (UnLockedNodes[i] == NodeID)
            {
                return false;
            }
        }
        UnLockedNodes.Add(NodeID);
        return true;
    }
    public bool AddstoreItem(string ID)
    {
        for(int i = 0;i< storeItemsID.Count;i++)
        {
            if (storeItemsID[i] == ID)
            {
                return false;
            }
        }
        storeItemsID.Add(ID);

        return true;
    }
    public bool GetLevelFinishData(string LevelName,degreetype type)
    {
        for(int i = 0;i<_levelFinishData.Count;i++)
        {
            if (_levelFinishData[i].LevelName == LevelName)
            {
                if(type == degreetype.normal)
                {
                    return _levelFinishData[i].finishTheNormalLevel;
                }
                if(type == degreetype.hard)
                {
                    return _levelFinishData[i].finishTheHardLevel;
                }
                if(type == degreetype.hell)
                {
                    return _levelFinishData[i].finishTheHelllLevel;
                }
            }
        }
        return false;
    }
    public bool GetLevelFinishData(string LevelName)
    {
        for (int i = 0; i < _levelFinishData.Count; i++)
        {
            if (_levelFinishData[i].LevelName == LevelName)
            {
         
                    return _levelFinishData[i].finishTheNormalLevel|| _levelFinishData[i].finishTheHardLevel|| _levelFinishData[i].finishTheHelllLevel;
            
            }
        }
        return false;
    }


    public void SaveOverWorldInfo(OverWorldInfo info)
    {
       
        for (int i = 0; i < overWorldInfos.Count; i++)
        {
            if (overWorldInfos[i].SceneName == info.SceneName)
            {
                overWorldInfos[i] = info;
                return;
            }
        }
        overWorldInfos.Add(info);
        return;
    }
}
[System.Serializable]
public class LevelFinishData//关卡完成数据
{
    public string LevelName;
    public bool finishTheNormalLevel;
    public bool finishTheHardLevel;
    public bool finishTheHelllLevel;
}