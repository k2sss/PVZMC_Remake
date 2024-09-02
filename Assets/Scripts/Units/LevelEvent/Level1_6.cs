using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    
    public class Level1_6 : BaseLevelEvent
    {
        public EnemyManager enemyMgr;
        private void Start()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.normal)
            {
                enemyMgr.EnemySpeedAdder = -0.2f;
                PlantManager.Instance.AttackSpeedAdder = +1f;
                PlantManager.Instance.DamageAdder = 0.2f;
            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                enemyMgr.EnemySpeedAdder = -0.2f;
                PlantManager.Instance.AttackSpeedAdder = +1f;
                PlantManager.Instance.DamageAdder = 0.2f;
                SunMore = false;
            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                enemyMgr.EnemySpeedAdder = -0.2f;
                PlantManager.Instance.AttackSpeedAdder = +1f;
                PlantManager.Instance.DamageAdder = 0.2f;

                for (int i = 0; i < enemyMgr.enemycreateInfos.Count; i++)
                {

                    if (enemyMgr.enemycreateInfos[i].type == EnemyType.zombie_sworder)
                    {
                        enemyMgr.enemycreateInfos[i].Weight = 10000;
                    }
                }
                SworderCrazyMode = true;
            }
            EventMgr.Instance.AddEventListener("GameStart", WhenGameStart);
            EventMgr.Instance.AddEventListener("GameWin", WhenGameWIn);
        }
        public void WhenGameStart()
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>().SandStormEffected = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>().WindStrength = -1.5f;

        }
        public void WhenGameWIn()
        {
            MySystem.Instance.nowUserData.AddstoreItem("≤£¡ß∆ø");
            MySystem.Instance.nowUserData.AddstoreItem("∑¢ΩÕ÷©÷Î—€");
            MySystem.Instance.SaveNowUserData();
        }

    }
}

