using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerBuff : MobBuff
{
    public static PlayerBuff Instance;
    public GameObject Dlight;
    private PlayerHealth player;
    private GameObject QuestionParticle;
    private GameObject targetQuestionParticle;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    protected override void InitBuffFunc()
    {
        base.InitBuffFunc();
        if (player == null)
            player = PlayerHealth.instance;
        QuestionParticle = Resources.Load<GameObject>("Particles/QuestionParticle");

         //hungry
         buffs[1].onStartWork += HungerManager.instance.InsideSpriteBecomeHungry;
        buffs[1].onStartWork += () => PlayerHunger.Instance.IsHungry = true;
        buffs[1].onStopWork += HungerManager.instance.InsideSpriteBecomeNormal;
        buffs[1].onStopWork += () => PlayerHunger.Instance.IsHungry = false;
        //poison
        buffs[3].onStartWork += HeartManager.instance.InsideSpriteBecomePoisoned;
        buffs[3].onStopWork += HeartManager.instance.InsideSpriteBecomeNormal;
        //Glowing
        buffs[4].onStartWork += () => Dlight.SetActive(true);
        buffs[4].onStopWork += ()=>Dlight.SetActive(false);

        buffs[13].onStartWork += () =>
        {
            PlayerMoveController.Instance.IsReverse = true;
            targetQuestionParticle = Instantiate(QuestionParticle, transform);
            targetQuestionParticle.transform.position = transform.position + Vector3.up;
            player.SetNowSpeed(0.4f);
        };
        buffs[13].onStopWork += () =>
        {
            PlayerMoveController.Instance.IsReverse = false;
            Destroy(targetQuestionParticle);
            player.SetNowSpeed(-0.4f);
        };
        buffs[14].onStartWork += () =>
        {
            player.natureAddHealthValue += 1;
        };
        buffs[14].onStopWork += () =>
        {
            player.natureAddHealthValue -= 1;
        };
      
    }


}

