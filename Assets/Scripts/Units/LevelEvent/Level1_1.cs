using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{ 
public class Level1_1 : BaseLevelEvent
{
        public EnemyManager enemyMgr;
    void Start()
    {
            if(MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                enemyMgr.k *= 2f;
                enemyMgr.b *= 2f;
            }
            else
            if(MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                enemyMgr.EnemyHealthMultiplier = 4;

            }


       EventMgr.Instance.AddEventListener("GameWin", WhenGameWin);
    }
    public void WhenGameWin()
        {
        if(MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.sunFlower))
            WhenGetPlantPannel.Instance.Show(PlantsType.sunFlower);
            MySystem.Instance.SaveNowUserData();
        }
}

}

