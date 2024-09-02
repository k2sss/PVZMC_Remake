using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CraftTablePannel : Pannel
{
    public static CraftTablePannel Instance;
    public override void Init()
    {
        base.Init();
        Instance = this;
        if (MySystem.IsInLevel())
        {
            UIMgr.Instance.AddPushAction(PannelName, () => Time.timeScale = 0.05f);
            UIMgr.Instance.AddPopAction(PannelName, () => Time.timeScale = 1);
        }
        else
        {
            UIMgr.Instance.AddPopAction(PannelName, ()=>MySystem.Instance.SaveNowUserData());
        }
    
        gameObject.SetActive(false);
    }
    public void Close()
    {
        
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if (!MySystem.IsInLevel())
        {
            PopUI();
        }
    }

}
