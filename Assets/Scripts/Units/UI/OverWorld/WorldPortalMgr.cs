using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPortalMgr : MonoBehaviour
{
    public InGameMenuInfos infos;
    public GameObject element;
    public Transform targetParent;
    public Pannel pannel; 
    private void Start()
    {
        pannel = GetComponent<Pannel>();
        Init();
    }
    public void Init()
    {
        //clear
        for (int i = 0; i < targetParent.childCount; i++)
        {
            Destroy(targetParent.GetChild(i));
        }

        for (int i = 0; i < infos.list.Count; i++)
        {
            if (infos.list[i].SceneName != SceneMgr.Instance.GetSceneName())
            {
                GameObject el = Instantiate(element, targetParent);
                el.GetComponent<OverWorld.OverWorldPortalElement>().Init(infos.list[i]);
            }
        }
    }
    public void POp()
    {
        pannel.PopUI();
        
    }
}
