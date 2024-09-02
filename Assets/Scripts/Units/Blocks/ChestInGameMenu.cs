using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInGameMenu : MonoBehaviour
{
    public int ChestID;
    private ChestInGameMenuPannel chestscript;
    private void Start()
    {
        chestscript = UIMgr.Instance.GetUIObject("Chest_InGameMenu").GetComponent<ChestInGameMenuPannel>();
    }
    private void OnMouseEnter()
    {
        gameObject.GetComponent<Animator>().SetBool("Open", true);
    }
    private void OnMouseExit()
    {
        gameObject.GetComponent<Animator>().SetBool("Open", false);
    }
    public void OpenChest()
    {
        if (UIMgr.Instance.IsUIActive("Store")) return;

        UIMgr.Instance.PushUIByKey("Chest_InGameMenu");
        chestscript.currentID = ChestID;
        switch (ChestID)
        {
            case 0:
                chestscript.GetSavedData(MySystem.Instance.nowUserData.items_inChest);
                break;
            case 1:
                chestscript.GetSavedData(MySystem.Instance.nowUserData.items_inChest1);
                break;
        }

        //Debug.Log(ChestID);
    }
}
