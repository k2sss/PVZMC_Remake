using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level2_5 : BaseLevelEvent
    {
        public Animator CameraAnimator;
        public GameStateMgr StateMgr;
        public EnemyManager enemyManager;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            StateMgr.EnterGameDirectly = true;
            if (MySystem.getUserData().LevelType == degreetype.hard)
            {
                enemyManager.enemycreateInfos.Add(new EnemyCreateInfo(3,EnemyType.EnderMan,1,4000,0));
                enemyManager.enemycreateInfos.Add(new EnemyCreateInfo(5, EnemyType.Slime_Middle, 1, 4000, 0));
            }
        }
        private void Start()
        {
            EventMgr.Instance.AddEventListener("GameStart", () => CameraAnimator.enabled = false);

                StateMgr.UnableRCard();
                CameraAnimator.enabled = true;
                MonoController.Instance.Invoke(4, () => StateMgr.StartWithNoRCard());
                UIMgr.Instance.GetUIObject("CardSlot").gameObject.SetActive(false);
            if (MySystem.getUserData().LevelType == degreetype.normal)
            {
                UIMgr.Instance.GetUIObject("CardSlot_Trans").transform.GetChild(0).GetComponent<CardSlot_Trans>().Init(5, new PlantsType[] { PlantsType.rolling_nuts,
                 PlantsType.rolling_nuts,PlantsType.rolling_nuts,PlantsType.rolling_nuts_bomb,PlantsType.rolling_nuts_big});
            }
            else
            {
                UIMgr.Instance.GetUIObject("CardSlot_Trans").transform.GetChild(0).GetComponent<CardSlot_Trans>().Init(5, new PlantsType[] {  PlantsType.rolling_nuts,PlantsType.rolling_nuts,
                    PlantsType.rolling_nuts, PlantsType.rolling_nuts
                ,PlantsType.rolling_nuts_bomb,PlantsType.rolling_nuts_big});

                
            }
        }

    }

}