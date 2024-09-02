using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aloes : Plants
{
    public AudioClip treatSound;
    
    protected override void Start()
    {
        base.Start();
       
    }
    protected override void Update()
    {
        base.Update();
        Check();
    }
    public void Attack()
    {
        //for(int i =targetGrid.x - 1;i< targetGrid.x +2;i++)
        //    for (int j = targetGrid.y - 1; j < targetGrid.y + 2; j++)
        //    {
        //            Plants target = GridManager.Instance.GetPlant(i, j);
        //            if(target!=null&&target.Health<target.MaxHealth)
        //            target.GetTreatment(FinalDamage);
        //    }

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Ray ray = new Ray(transform.position + new Vector3(i * 2, 3, j * 2), Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit hit, 5, 1 << 9))
                {
                    Plants p = hit.collider.GetComponent<Plants>();
                    if (p != null)
                    {
                        if (p.Health < p.MaxHealth)
                        {
                            p.GetTreatment(FinalDamage);
                        }
                    }
                }

            }
        }

        if (WithInRange(2, GameObject.FindGameObjectWithTag("Player").transform) && PlayerHealth.instance.Health < PlayerHealth.instance.MaxHealth)
            PlayerHealth.instance.GetTreatment(FinalDamage / 2);
        PlayOneShot(treatSound,0.3f);
    }

    private void Check()
    {

        bool flag = false;
       /* for (int i = targetGrid.x - 1; i < targetGrid.x + 2; i++)
            for (int j = targetGrid.y - 1; j < targetGrid.y + 2; j++)
            {
                Plants target = GridManager.Instance.GetPlant(i, j);
                if (target != null)
                {
                    if (target.Health < target.MaxHealth)
                    {
                        flag = true;
                    }
                }
            }*/
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Ray ray = new Ray(transform.position + new Vector3(i * 2, 3, j * 2), Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit hit, 5, 1 << 9))
                {
                    Plants p = hit.collider.GetComponent<Plants>();
                    if (p != null)
                    {
                        if (p.Health < p.MaxHealth)
                        {
                            flag = true;
                        }
                    }
                }

            }
        }



        if (WithInRange(2, GameObject.FindGameObjectWithTag("Player").transform)&&PlayerHealth.instance.Health<PlayerHealth.instance.MaxHealth)
        {
            flag = true;
        }

        if (flag == true)
        {
            IsAttacking = true;
            _animator.SetBool("Attack",true);
        }
        else
        {
            IsAttacking = false;
            _animator.SetBool("Attack",false);
        }
    }
   
}
