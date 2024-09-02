using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
public class Level3_6 : BaseLevelEvent
{
        public EnemyManager enemyMgr;
    // Start is called before the first frame update
    void Start()
    {
            degreetype ltype = GetLevelType();
            ShowDave(new string[]
            {
            "我去，是苦力怕！",
            "这次它换了个颜色？",
            "最好不要去惹爆这个暴脾气"

            });
            GetPlantWhenWin(PlantsType.GlowStoneHander);
            AddStoreItem("生命水晶");

            if(ltype == degreetype.hard)
            {
                enemyMgr.EnemySpeedAdder = -0.3f;
                enemyMgr.EnemyHealthMultiplier = 2f;
            }
    }

}
}

