using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Cactus : Plants
{
    public float Range;
    private float Timer;
    private float TimeSleep = 0.5f;
    protected override void Update()
    {
        base.Update();
        Timer += Time.deltaTime;
        if (Timer > TimeSleep)
        {
            Timer = 0;
            Attack();
        }
    }
    private void Attack()
    {
        GameObject[] gs = FindTheNearestMobs("Enemy", Range);
        for (int i = 0; i < gs.Length; i++)
        {
            gs[i].GetComponent<Enemy>().Hurt(FinalDamage);
        }
    }
    
}
