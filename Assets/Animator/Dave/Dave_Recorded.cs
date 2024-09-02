using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dave_Recorded : MonoBehaviour
{
    private Animator thisanimator;
    private bool isInited;

    public void Init()
    {
        if (!isInited)
        {
            isInited = true;
            thisanimator = GetComponent<Animator>();
        }
    }
    public void ChangeToZero()
    {
        Init();
        thisanimator.SetInteger("Talk", 0);
    }
    
}
