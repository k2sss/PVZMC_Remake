using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : ListImageUIManager<HeartManager>
{
    public GameObject[] hearts;
    public PlayerHealth ph;
    public float HighLightOnceTime;
    public ListImageSpriteInfo[] Ready_SpriteInfo;
    private GridLayoutGroup group;
    public void Start()
    {

        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        EventMgr.Instance.AddEventListener("max_hp_change", ()=>
        {
            UpdateHp();
            AllInfoUpdate();

        });
        for (int i = 0; i < hearts.Length; i++)
        {
            Init(hearts[i]);
        }
        UpdateHp();
        //ph.onPlayerHealthChange += UpdateHeartInfo;
        ph.onPlayerHealthChange += AllInfoUpdate;
        ph.onPlayerHealthChange += MakeAllHightLight;
        ph.onPlayerHealthChange += CheckIsShake;
        AllInfoUpdate();
    }

    public void UpdateHp()//¸üÐÂÑªÁ¿
    {
        DeleteAll();
        for (int i = 0; i < MyCompute((int)ph.MaxHealth);i++)
        {
            Add(hearts[0]);
        }
        for (int i = 0; i < MyCompute((int)ph.MaxGoldenHealth); i++)
        {
            Add(hearts[1]);
        }
    }
    public void AllInfoUpdate()
    {
        InfoUpdate((int)ph.Health, hearts[0]);
        InfoUpdate((int)ph.GoldenHealth, hearts[1]);
    }
    public void CheckIsShake()
    {
        if (ph.Health <= 6)
        {
            MakeAGroupShake(hearts[0]);
        }
        else
        {
            MakeAGroupStopShake(hearts[0]);
        }
    }
    public void MakeAllHightLight()
    {
        MakeAllHighLightOnce(HighLightOnceTime);
    }
    public void InsideSpriteBecomePoisoned()
    {
        MakeAGroupChangeSprite(hearts[0], Ready_SpriteInfo[1]);
    }
    public void InsideSpriteBecomeNormal()
    {
        MakeAGroupSpriteDeafault(hearts[0]);
    }

}