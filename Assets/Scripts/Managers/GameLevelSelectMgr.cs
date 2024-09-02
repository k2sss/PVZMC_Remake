using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuMgr : BaseManager<InGameMenuMgr>
{
    public Animator CameraAnimator;
    public ButtonTwoFunc SignButton;

    private void Start()
    {
        UIMgr.Instance.UnAbleUI("Game");
        UIMgr.Instance.UnAbleUI("Inventory");
        //UIMgr.Instance.ShowUI("LevelSelect");
    }


    public void EnterLevelSelect()
    {
        UIMgr.Instance.ShowUI("LevelSelect");
        CameraAnimator.SetBool("Select", true);
        
    }
    public void ExitLevelSelect()
    {
        UIMgr.Instance.CloseUI("LevelSelect");
        CameraAnimator.SetBool("Select", false);
        CameraAnimator.SetBool("IsScroll", false);
        
    }
}
