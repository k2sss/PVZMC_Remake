using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIslotMgr : MonoBehaviour
{
    private void Start()
    {
        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.PlayerDisappear == true)
        {
            gameObject.SetActive(false);
        }
        else
        if (MySystem.IsInLevel())
        {
            gameObject.SetActive(false);
            
            EventMgr.Instance.AddEventListener("GameStart", () => gameObject.SetActive(true));
            EventMgr.Instance.AddEventListener("PickUpChest", () => gameObject.SetActive(false));
        }

    }
    public void OpenBag()
    {
        UIMgr.Instance.PushUIByKey("InventoryPannel");
    }
    public void OpenSet()
    {
        UIMgr.Instance.PushUIByKey("Menu");
    }



}
