using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCarMoveable : MonoBehaviour
{
    private Rigidbody rb;
    public float MoveSpeed = 2;
    public Vector3 InputDir;
    public Plants[] plants;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //EventMgr.Instance.AddEventListener("GameStart",()=> moveable = true);
        //EventMgr.Instance.AddEventListener("GameOver", () => moveable = false);
        EventMgr.Instance.AddEventListener("ChangeWave", PlantsUpdate);

    }
    private void PlantsUpdate()
    {
        int count = LevelManager.Instance.nowWave;
        Debug.Log(count);
        switch (count)
        {
            //case 1:
            //    AllPlantsAttackSpeedUp(1f);
            //    break;

            case 2:
                AllPlantsAttackSpeedUp(1f);
                break;
            case 3:
                ChangePlant(1);
                AllPlantsAttackSpeedUp(1f);
                break ;
            case 4:
                AllPlantsAttackSpeedUp(1f);
                break;
            case 5:
                ChangePlant(2);
                break;
            case 6:
                ChangePlant(3);
                AllPlantsDamageUp(0.5f);
                break;
            case 7:
                AllPlantsDamageUp(0.5f);
                break;
            case 8:
                ChangePlant(4);
                    break;
            case 10:
                ChangePlant(5);
                break;
            case 11:
                ChangePlant(6);
                AllPlantsAttackSpeedUp(-2f);
                AllPlantsDamageUp(1f);
                break;
           
        }


    }
    public Plants ChangePlant(int index)
    {
        for (int i = 0; i < plants.Length; i++)
        {
            plants[i].gameObject.SetActive(false);
        }
        plants[index].gameObject.SetActive(true);
        return plants[index];
    }
    public void AllPlantsAttackSpeedUp(float n)
    {
        for (int i = 0; i < plants.Length; i++)
        {
            plants[i].SetAttackSpeed(n);
        }
    }
    public void AllPlantsDamageUp(float n)
    {
        for (int i = 0; i < plants.Length; i++)
        {
            plants[i].SetNowDamage(n);
        }
    }
    private void Update()
    {
       
       

            if (PhoneControlMgr.PhoneControl == true)
            {
                Vector2 a = PhoneControlMgr.Instance.handler.OutPut;
                InputDir = new Vector3(a.x, 0, a.y);
            }
            else
            {
                InputDir = new Vector3(InputMgr.GetAxisRaw("Horizontal"), 0, InputMgr.GetAxisRaw("Vertical"));
            }

    }
    private void FixedUpdate()
    {
        Move(InputDir);
    }
    public void Move(Vector3 direction)
    {
        Vector3 dir = direction.normalized * MoveSpeed;
        rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z);
    }




}
