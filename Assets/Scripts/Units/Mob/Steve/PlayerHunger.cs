using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHunger : BaseManager<PlayerHunger>
{
    public int Hunger;//饥饿值
    public int MaxHunger;
    public int Hunger_over;//饱食度
    public bool IsHungry;
    private float HungerTimer;
    private float HungerTimer2;
    private float HungerFactor;
    private bool DoNotConsumeHunger;
    public Action onHungerChange { get; set; }
    public Action onEatFood;
    private PlayerHealth playerhealth;
    private MobBuff buffloader;
    private void Start()
    {
        EventMgr.Instance.AddEventListener("DisableHunger", () => DoNotConsumeHunger = true);
        playerhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        buffloader = playerhealth.mobBuffer;
    }
    private void Update()
    {
        CheckHunger();
        if (IsHungry == true)
        {
            AddHunger(1 * Time.deltaTime);
        }
    }
    public void EatFood(int hunger, int hunger_over)
    {
        Hunger += hunger;
        if (Hunger > MaxHunger)
            Hunger = MaxHunger;

        Hunger_over += hunger_over;
        if (Hunger_over > Hunger)
            Hunger_over = Hunger;
        onEatFood?.Invoke();
    }
    public void AddHunger(float factor)
    {
        if (buffloader.IsContainBuff(BuffType.Satiety)) factor *= 0.1f;
        

        if (MySystem.IsInLevel())
        {
            HungerFactor += factor;
            if (HungerFactor >= 1 && DoNotConsumeHunger == false)
            {
                HungerFactor = 0;
                if (Hunger_over > 0)
                {
                    Hunger_over -= 1;
                }
                else
                {
                    Hunger -= 1;
                }
                if (Hunger_over < 0)
                    Hunger_over = 0;
                if (Hunger < 0)
                    Hunger = 0;

                onHungerChange?.Invoke();
            }
        }
    }//添加 饥饿因子
    private void CheckHunger()
    {

        if (PlayerHealth.instance.Health < PlayerHealth.instance.MaxHealth)//当血量不满时
        {
            HungerTimer += Time.deltaTime;
            if (Hunger_over > 0)
            {
                if (HungerTimer > 0.5f)
                {
                    AddHunger(1);
                    FoodAddHealth();
                    HungerTimer = 0;
                }

            }
            else if (Hunger > 18)
            {
                if (HungerTimer > 4)
                {
                    AddHunger(1);
                    FoodAddHealth();
                    HungerTimer = 0;
                }
            }
            else
            {
                HungerTimer2 += Time.deltaTime;
                if (HungerTimer2 > 25)
                {
                    AddHunger(1);
                    HungerTimer2 = 0;

                }
            }
        }
        else//当血量满时，正常timer
        {
            HungerTimer2 += Time.deltaTime;
            if (HungerTimer2 > 25)
            {
                AddHunger(1);
                HungerTimer2 = 0;
            }

        }



    }//
    private void FoodAddHealth()//通过饱食度机制回血
    {
        PlayerHealth.instance.AddHealth(1);
    }

}
