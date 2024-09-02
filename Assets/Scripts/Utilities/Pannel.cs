using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pannel : MonoBehaviour
{
    public string PannelName;
    public bool NotHide;
    public bool CantPop;
    public bool IsOpen;
    public bool HideFrist;
    public virtual void Init()//设置这个面片的初始化信息
    {
        if (GetComponent<Canvas>() != null)
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        UIMgr.Instance.Regist(PannelName, gameObject,NotHide,CantPop);
        UIMgr.Instance.AddPushAction(PannelName, () => IsOpen = true);
        UIMgr.Instance.AddPopAction(PannelName, () => IsOpen = false);

        if(HideFrist == true)
        {
            gameObject.SetActive(false);
        }
    }
    public virtual void PopUI()
    {
        //if (UIMgr.Instance.UIs[UIMgr.Instance.UIs.Count - 1].name == PannelName)
        UIMgr.Instance.PopUI();
    }
}
