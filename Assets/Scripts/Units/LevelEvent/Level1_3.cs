using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level1_3 : BaseLevelEvent
    {

        public EnemyManager enemyMgr;
        void Start()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                enemyMgr.EnemyDamageAdder = 9f;
            }
            else
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                enemyMgr.EnemySpeedAdder = 1f;
                enemyMgr.EnemyDamageAdder = 0.2f;
                enemyMgr.EnemyAttackSpeedAdder = 0.4f;
                PlantManager.Instance.AttackSpeedAdder = -0.2f;
                EventMgr.Instance.AddEventListener("GameStart", WhenGameStart);
            }
            
            EventMgr.Instance.AddEventListener("GameWin", WhenGameWin);

        }
        public void WhenGameStart()
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>().SandStormEffected = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>().WindStrength = 1.5f;

        }
        public void WhenGameWin()
        {
            MySystem.Instance.nowUserData.AddstoreItem("“∂¬Ã¿·");
            MySystem.Instance.SaveNowUserData();
        }
    }
}
