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
            "��ȥ���ǿ����£�",
            "��������˸���ɫ��",
            "��ò�Ҫȥ�Ǳ������Ƣ��"

            });
            GetPlantWhenWin(PlantsType.GlowStoneHander);
            AddStoreItem("����ˮ��");

            if(ltype == degreetype.hard)
            {
                enemyMgr.EnemySpeedAdder = -0.3f;
                enemyMgr.EnemyHealthMultiplier = 2f;
            }
    }

}
}

