using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBlock : MonoBehaviour
{
   public void OpenUI()
    {
        if (UIMgr.Instance != null)
        {
            UIMgr.Instance.PushUIByKey("WorldPortal");
            
        }
    }

}
