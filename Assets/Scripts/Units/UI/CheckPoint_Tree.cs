using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint_Tree : MonoBehaviour
{
    public Transform PlayerTransform;
    public float Range = 10;
    public bool push;

    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (push == false &&(PlayerTransform.position - transform.position).magnitude <= Range)
        {
            push = true;
            UIMgr.Instance.ShowUI("LevelSelect");
        }
        else if(push == true && (PlayerTransform.position - transform.position).magnitude > Range)
        {
            push = false;
            UIMgr.Instance.CloseUI("LevelSelect");
        }
    }
}
