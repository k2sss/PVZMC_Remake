using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryFlower : Plants
{
    private float timer;
    protected override void Update()
    {
        base.Update();
        HurtRig();
        timer += Time.deltaTime;
        if (timer > 2)
        {
            timer = 0;
            PickSun();
        }
    }
   
    public void PickSun()
    {
        GameObject[] suns = GameObject.FindGameObjectsWithTag("Sun");
        for (int i = 0; i < suns.Length; i++)
        {
            Sun sun = suns[i].GetComponent<Sun>();
            if (sun._selected == false)
            {
                sun.Pick();
                break;
            }
        }
    }

}
