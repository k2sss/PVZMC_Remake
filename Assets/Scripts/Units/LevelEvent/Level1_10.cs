using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level1_10 : BaseLevelEvent
    {
       public Animator CameraAnimator;
       public GameStateMgr StateMgr;
        public GameObject BossEnemy;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            if (MySystem.getUserData().LevelType == degreetype.normal)
            {
                StateMgr.EnterGameDirectly = true;
            }
            else if (MySystem.getUserData().LevelType == degreetype.hard)
            {
                StateMgr.EnterGameDirectly = true;
                PlayerAttackHurt = true;
            }
        }
        private void Start()
        {
            EventMgr.Instance.AddEventListener("GameStart", () => CameraAnimator.enabled = false);
            BossEnemy.SetActive(false);
            EventMgr.Instance.AddEventListener("GameStart", () => BossEnemy.SetActive(true));
            if (MySystem.getUserData().LevelType == degreetype.normal)
            {
                StateMgr.UnableRCard();
                CameraAnimator.enabled = true;
                MonoController.Instance.Invoke(4, () => StateMgr.StartWithNoRCard());
                UIMgr.Instance.GetUIObject("CardSlot").gameObject.SetActive(false);
                UIMgr.Instance.GetUIObject("CardSlot_Trans").transform.GetChild(0).GetComponent<CardSlot_Trans>().Init(5, new PlantsType[] { PlantsType.peaShooter
                ,PlantsType.Cactus,PlantsType.potatoMine,PlantsType.DryShooter,PlantsType.SandShooter,PlantsType.GasterBlaster,PlantsType.PeaPitcher});
            }
            else if (MySystem.getUserData().LevelType == degreetype.hard)
            {
                StateMgr.UnableRCard();
                CameraAnimator.enabled = true;
                MonoController.Instance.Invoke(4, () => StateMgr.StartWithNoRCard());
                UIMgr.Instance.GetUIObject("CardSlot").gameObject.SetActive(false);
                UIMgr.Instance.GetUIObject("CardSlot_Trans").transform.GetChild(0).GetComponent<CardSlot_Trans>().Init(5, new PlantsType[] { PlantsType.peaShooter
                ,PlantsType.Cactus,PlantsType.potatoMine,PlantsType.DryShooter,PlantsType.SandShooter,PlantsType.GasterBlaster,PlantsType.PeaPitcher});
            }
            else if (MySystem.getUserData().LevelType == degreetype.hell)
            {
                CardSlot.Instance.SetSunCount(1000);
            }
        }

    }
}