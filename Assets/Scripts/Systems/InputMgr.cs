using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputMgr : BaseManager<InputMgr>
{
    public static bool CanInput;
    protected override void Awake()
    {
        base.Awake();
        CanInput = true;
    }


    public static bool GetKeyDown(KeyCode code)
    {
        if (CanInput == false)
        {
            return false;
        }
        return Input.GetKeyDown(code);
    }
    public static bool GetKey(KeyCode code)
    {
        if (CanInput == false)
        {
            return false;
        }
        return Input.GetKey(code);
    }
    public static bool GetKeyUp(KeyCode code)
    {
        if (CanInput == false)
        {
            return false;
        }
        return Input.GetKeyUp(code);
    }
    public static float GetAxisRaw(string axis)
    {
        if (CanInput == false)
        {
            return 0;
        }
        return Input.GetAxisRaw(axis);
    }

    public static bool GetMouseButtonDown(int mouseType)
    {
        
        if (CanInput == false)
        {   
            return false;
        }
      
        return Input.GetMouseButtonDown(mouseType);
    }
    public static bool GetMouseButton(int mouseType)
    {
        if (CanInput == false)
        {
            return false;
        }
       
        return Input.GetMouseButton(mouseType);
    }

    public void EnableAllInput()//启动所有键盘输入
    {
        CanInput = true;
    }
    public void UnableAllInput()//禁用所有键盘输入
    {
        CanInput = false;
    }
}