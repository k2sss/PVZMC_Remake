using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneControlMgr : Pannel
{
    public static PhoneControlMgr Instance { get; private set; }
    public HandlerPannel handler;
    public PhoneInputButton[] buttons;
    private float clickTwiceTimer;
    private Vector3 preClickPos;
    public static bool PhoneControl;
    public bool UnAble { set; get; }
    private bool flag;
    private void Awake()
    {
        Instance = this;
        if (Application.platform == RuntimePlatform.Android)
        {
            PhoneControl = true;

        }
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            PhoneControl = false;
        }
    }
    public void SetActiveToF(bool b)
    {
        if (PhoneControl == false)
        {
            b = false;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(b);
        }
    }
    void Start()
    {
        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.HidePhoneButton == true)
        {
             UnAble = true;
        }
        else
        {
            if (MySystem.IsInLevel())
            {
                EventMgr.Instance.AddEventListener("GameStart", () => SetActiveToF(true));
                EventMgr.Instance.AddEventListener("GameStart", () => UnAble = false);
                EventMgr.Instance.AddEventListener("GameOver", () => UnAble = true);
                EventMgr.Instance.AddEventListener("PickUpChest", () => UnAble = true);
                SetActiveToF(false);
            }
            if (PhoneControl == false)
            {
                SetActiveToF(false);
            }
            MonoController.Instance.HalfTickAction += () => ShowCraftTableButton(false);
            ShowCraftTableButton(false);

        }

    }
    private void Update()
    {
        clickTwiceTimer -= Time.unscaledDeltaTime;
        //flag保证这段代码后执行
        flag = false;
        if (InputMgr.GetMouseButtonDown(0) && clickTwiceTimer < 0)
        {
            preClickPos = Input.mousePosition;
            clickTwiceTimer = 0.4f;
            flag = true;
        }


        if (UnAble == true)
        {
            SetActiveToF(false);
        }
        if (buttons[2].IsClicked())
        {
            UIMgr.Instance.PushUIByKey("CraftTable");
        }

    }

    public bool IsAttackButtonDown()
    {
        return buttons[0].IsClicked();
    }
    public bool IsAttackButtonPressed()
    {
        return buttons[0].IsPressed();
    }
    public bool IsJumpButtonDown()
    {
        return buttons[1].IsClicked();
    }
    public bool IsJumpButtonPressed()
    {
        return buttons[1].IsPressed();
    }

    public void ShowCraftTableButton(bool a)
    {
        buttons[2].gameObject.SetActive(a);
    }
    public bool ClickTwice()
    {
        if (clickTwiceTimer > 0 && InputMgr.GetMouseButtonDown(0) && (Input.mousePosition - preClickPos).magnitude < 100 && flag == false)
        {
            //Debug.Log((Input.mousePosition - preClickPos).magnitude);
            Debug.Log("cliclTwice");
            return true;

        }
        return false;

    }

    public void CloseJumpAndAttackButton(bool c)
    {
        if (PhoneControl == true)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(!c);
            }
        }

    }
}
