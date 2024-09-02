using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level1_2 : BaseLevelEvent
    {

        public EnemyManager enemyMgr;
        void Start()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                enemyMgr.EnemySpeedAdder = 0.5f;
            }
            else
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                PlantManager.Instance.AllPlantsHPtoOne = true;
                enemyMgr.EnemySpeedAdder = 1f;
            }
            EventMgr.Instance.AddEventListener("GameWin", WhenGameWin);
        }
        public void WhenGameWin()
        {
            if (MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.potatoMine))
                WhenGetPlantPannel.Instance.Show(PlantsType.potatoMine);
            MySystem.Instance.nowUserData.AddstoreItem("π‚Àÿ");

            MySystem.Instance.SaveNowUserData();
        }


    }
}