using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryPannel : Pannel
{
    public static InventoryPannel Instance;
    

    public override void Init()
    {
        base.Init();
        
        Instance = this;
        if (MySystem.IsInLevel())
        {
           UIMgr.Instance.AddPushAction(PannelName, () => Time.timeScale = 0f);
           UIMgr.Instance.AddPopAction(PannelName, () => Time.timeScale = 1);
        }
       
       
        
    }
    private void Start()
    {
       
         gameObject.SetActive(false);
    }


}
