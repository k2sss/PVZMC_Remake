using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{
    public class Level3_11 : BaseLevelEvent
    {
        public GameStateMgr stageMgr;
        public Animator CameraAnimator;
        private degreetype type;
        protected override void Awake()
        {
            type = MySystem.getUserData().LevelType;
            base.Awake();
            if(type == degreetype.normal)
            stageMgr.EnterGameDirectly = true;
            
        }
        private void Start()
        {
            
            SunManager.Instance.FallSun = false;

            if (type == degreetype.normal)
            {
                EventMgr.Instance.AddEventListener("GameStart", () => CameraAnimator.enabled = false);
                stageMgr.UnableRCard();
                CameraAnimator.enabled = true;
                MonoController.Instance.Invoke(4, () => stageMgr.StartWithNoRCard());
                UIMgr.Instance.GetUIObject("CardSlot").gameObject.SetActive(false);
                UIMgr.Instance.GetUIObject("CardSlot_Trans").transform.GetChild(0).GetComponent<CardSlot_Trans>().Init(8, new PlantsType[] { PlantsType.ChargingShooter
                ,PlantsType.Crimson_Flower,PlantsType.Soul_Breaker,PlantsType.flowerPot,PlantsType.PurifyCherry,PlantsType.BloodChomper,PlantsType.PeaPitcher
            ,PlantsType.GasterBlaster,PlantsType.nuts});
            }
            else if (type == degreetype.hard)
            {
                CardSlot.Instance.SetSunCount(1000);
            }
        }
    }
}   
