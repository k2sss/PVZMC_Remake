using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton_InLevel : MonoBehaviour
{
    public void OpenMenu()
    {
        UIMgr.Instance.PushUIByKey("Menu");
    }
}
