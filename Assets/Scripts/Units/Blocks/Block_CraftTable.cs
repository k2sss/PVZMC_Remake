using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_CraftTable : FunctionalBlock
{
    private float btimer;
    private Transform PlayerTransform;
    protected override void Start()
    {
        base.Start();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void Update()
    {
        base.Update();
        
        if (PhoneControlMgr.PhoneControl == true)
        {
            //ÊÖ»ú°æ²Ù×÷
            if (BlockManager.Instance.target == this)
            {
                float distance = (PlayerTransform.position - transform.position).magnitude;
                if (distance < 6)
                {
                    PhoneControlMgr.Instance.ShowCraftTableButton(true);
                }
            }
        }
        else
        if (BlockManager.Instance.target == this)
        {
           if (InputMgr.GetMouseButtonDown(1))
            {
                UIMgr.Instance.PushUIByKey("CraftTable");
            }
        }
    }

}
