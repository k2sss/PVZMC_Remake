using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjMgr : BaseManager<SceneObjMgr>
{
    public GameObject[] craftTables;
    public GameObject[] chests;
    public GameObject[] chests1;
    public GameObject[] store;
    
    private void Start()
    {
        UpdateState();
    }
    public void UpdateState()
    {
        UnLock(craftTables, MySystem.Instance.nowUserData.UnLockCraftTable);
        UnLock(chests, MySystem.Instance.nowUserData.UnLockChest);
        UnLock(store, MySystem.Instance.nowUserData.UnLockStore);
        UnLock(chests1, MySystem.Instance.nowUserData.UnLockChest1);
    }
    private void UnLock(GameObject[] gameObjects,bool b)
    {
        ArraySetActive(false, gameObjects);
        if (b == true)
        {
            ArraySetActive(true, gameObjects);
        }
    }
    private void ArraySetActive(bool active, GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(active);
        }
    }
    public void BackToMainMenu()
    {
        SceneMgr.Instance.LoadAsync("MainTitle");
        
    }
    public void OpenUI_BasicConstruct()
    {
        if (UIMgr.Instance.IsUIActive("Store")) return;
        UIMgr.Instance.PushUIByKey("Basic");
    }
    public void GameSetting()
    {
        UIMgr.Instance.PushUIByKey("Setting");

    }
    public void OpenCraftTable()
    {
        if (UIMgr.Instance.IsUIActive("Store")) return;
        UIMgr.Instance.PushUIByKey("CraftTable");

    }
    public void GoToStore()
    {
        SceneMgr.Instance.LoadAsync("Store");
    }
}
