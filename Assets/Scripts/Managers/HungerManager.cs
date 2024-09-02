using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerManager : ListImageUIManager<HungerManager>
{
    public GameObject[] hungers;
    private PlayerHunger ph;
    public float HighLightOnceTime;
    public ListImageSpriteInfo[] Ready_SpriteInfo;
 
    public void Start()
    {
        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHunger>();
        for (int i = 0; i < hungers.Length; i++)
        {
            Init(hungers[i]);
        }
        Init(ph.MaxHunger);
        Invoke("DEBUG", 0.01f); 
        AllInfoUpdate();
    }
    private void DEBUG()
    {
        ph.onHungerChange += AllInfoUpdate;
        ph.onEatFood += MakeAllHightLight;
       
    }
    public void Init(int value)//更新饥饿/初始化
    {
        DeleteAll();
        for (int i = 0; i < MyCompute(value); i++)
        {
            Add(hungers[0]);
        }
    }
    public void AllInfoUpdate()
    {
      
        InfoUpdate(ph.Hunger, hungers[0]);
        if (ph.Hunger <= 6)
        {
            MakeAGroupShake(hungers[0]);
        }
        else
        {
            MakeAGroupStopShake(hungers[0]);
            
        }
    }
    public void MakeAllHightLight()
    {
        MakeAllHighLightOnce(HighLightOnceTime);
    }
    public void InsideSpriteBecomeHungry()
    {
        MakeAGroupChangeSprite(hungers[0],Ready_SpriteInfo[1]);
    }
    public void InsideSpriteBecomeNormal()
    {
        MakeAGroupSpriteDeafault(hungers[0]);
    }
   
}
