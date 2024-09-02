using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class UIMgr : BaseManager<UIMgr>
{
     public GameObject[] RootPannels;
     public List<UIStack> UIs = new List<UIStack>();
    [SerializeField]private List<UIStack> RigistUIs = new List<UIStack>();
    public int UIStackCount;
    public bool IsOpenStore { get;  set; }
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < RootPannels.Length; i++)
        {
            GameObject newUI = Instantiate(RootPannels[i], transform);
            newUI.GetComponent<Pannel>().Init();
        }
    }
    private void Update()
    {
       

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!PopUI())
                {
                    PushUIByKey("Menu");

                }
            }
            if (InputMgr.GetKeyDown(KeyCode.E))
            {
                if (CraftTablePannel.Instance.IsOpen == false)
                {
                if (InventoryPannel.Instance.IsOpen == false&& UIStackCount == 0)
                    {
                   
                       if(BaseLevelEvent.Instance!=null&&BaseLevelEvent.Instance.PlayerDisappear == true)
                       { }
                        else
                        PushUIByKey("InventoryPannel");
                    }
                    else
                    {
                        PopUI();
                    }
                }
                else
                {
                    PopUI();
                }
            }

        

    }
    public bool IsUIActive(string name)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                return ui.gameobject.activeSelf;
            }
        }
        return false;
    }
    public GameObject GetUIObject(string name)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                return ui.gameobject;
            }
        }
        return null;
    }
    private void PushUI(UIStack stack)
    {
       
        UIs.Add(stack);
        UIStackCount++;
        //只显示最上层
        if (UIs.Count - 2 >= 0 && UIs[UIs.Count - 2].NotHide == false)
        {
            UIs[UIs.Count - 2].gameobject.SetActive(false);
        }
        if (UIs.Count - 1 >= 0)
        {
            UIs[UIs.Count - 1].gameobject.SetActive(true); //top
            UIs[UIs.Count - 1].gameobject.GetComponent<Pannel>().IsOpen = true;
        }
       
        stack.OnPush?.Invoke();
    }
    public bool PopUI()
    {
        if (UIs.Count > 0)
        {
            if (UIs[UIs.Count - 1].CantPop == false)
            {

                UIStackCount--;
                UIs[UIs.Count - 1].gameobject.SetActive(false);
                UIs[UIs.Count - 1].gameobject.GetComponent<Pannel>().IsOpen = false;
                UIs[UIs.Count - 1].OnPop?.Invoke();//出BUG的点

                UIs.Remove(UIs[UIs.Count - 1]);
                if (UIs.Count > 0)
                {
                    UIs[UIs.Count - 1].gameobject.SetActive(true);
                }
            }

            return true;
            //表示成功弹出
        }
        else
        {
            return false;
        }


    }
    public void Regist(string name, GameObject Object, bool NotHide = false, bool CantPop = false)
    {
        //遍历判断是否已经注册
        for (int i = 0; i < RigistUIs.Count; i++)
        {
            if (RigistUIs[i].name == name)//如果已经注册过，则Return
            {
                return;
            }
        }
        RigistUIs.Add(new UIStack(Object, name, NotHide, CantPop));
    }
    public void PushUIByKey(string name)
    {
        if (IsOpenStore == true && name == "InventoryPannel")
        {
            return;
        }


        foreach (UIStack ui in RigistUIs)
        {
            
            if (ui.name == name && ui.gameobject.GetComponent<Pannel>().IsOpen == false)
            {
                
                PushUI(ui);
                break;
            }
        }
    }
    public void ShowUI(string name)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                ui.gameobject.SetActive(true);
                break;
            }
        }
    }
    public void CloseUI(string name)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                ui.gameobject.SetActive(false);
                break;
            }
        }
    }
    public void AddPushAction(string name, UnityAction action)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                ui.OnPush += action;
                break;
            }
        }
    }
    public void DeletePushAction(string name, UnityAction action)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                ui.OnPush -= action;
                break;
            }
        }
    }
    public void AddPopAction(string name, UnityAction action)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                ui.OnPop += action;
                break;
            }
        }
    }
    public void DeletePopAction(string name, UnityAction action)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                ui.OnPop -= action;
                break;
            }
        }
    }

    public void UnAbleUI(string name)
    {
        foreach (UIStack ui in RigistUIs)
        {
            if (ui.name == name)
            {
                ui.gameobject.SetActive(false);
                RigistUIs.Remove(ui);
                break;
            }
        }
    }
    [System.Serializable]
    public class UIStack
    {
        public GameObject gameobject;
        public string name;
        public UnityAction OnPush;
        public UnityAction OnPop;
        public bool NotHide;
        public bool CantPop;
        public UIStack(GameObject obj, string index, bool NotHide = false, bool CantPop = false)
        {
            gameobject = obj;
            this.name = index;
            this.NotHide = NotHide;
            this.CantPop = CantPop;
        }
    }
}