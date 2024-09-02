using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetBerry :Plants
{
    private float hurtTimer;
    public float CheckRange;
    protected override void Update()
    {
        base.Update();
        hurtTimer += Time.deltaTime;
        if (hurtTimer > 0.5f)
        {
            hurtTimer = 0;
           
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
           
            for (int i = 0; i < Enemys.Length; i++)
            {
              
                if ((Enemys[i].transform.position - transform.position).magnitude <= CheckRange)
                {
                    Enemys[i].GetComponent<Enemy>().Hurt(FinalDamage);
                }
            }
        }
    }
}
