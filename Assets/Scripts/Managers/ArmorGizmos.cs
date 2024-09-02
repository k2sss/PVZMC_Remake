using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorGizmos : ListImageUIManager<ArmorGizmos>
{
    public GameObject[] gizmos;
    [HideInInspector] public PlayerHealth ph;
    public void Start()
    {
        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        for (int i = 0; i < gizmos.Length; i++)
        {
            Init(gizmos[i]);
        }
        //EventMgr.Instance.AddEventListener("UpdateArmor", Init);
        //EventMgr.Instance.AddEventListener("UpdateArmor", AllInfoUpdate);
        Invoke("Init", 0.01f);
        Invoke("AllInfoUpdate", 0.01f);
    }

    public void Init()//¸üÐÂ
    {
        DeleteAll();
        if (ph.Defence != 0)
        {
            for (int i = 0; i < MyCompute(20); i++)
            {
                Add(gizmos[0]);
            }
        }

    }
    public void AllInfoUpdate()
    {
        
        InfoUpdate((int)ph.Defence, gizmos[0]);
    }
}
