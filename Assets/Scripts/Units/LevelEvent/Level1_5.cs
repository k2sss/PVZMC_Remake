using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{ 
public class Level1_5 : BaseLevelEvent
{
        
        public EnemyManager enemyMgr;
        public LevelManager levelMgr;
        public Grid[] grids;
        protected override void Awake()
        {
            base.Awake();
            levelMgr.FirstZombieAppearTime = 2;

         }
        void Start()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.normal)
            {

            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                ScorpionAttackSpeedUp = true;
                for (int i = 0; i < enemyMgr.enemycreateInfos.Count; i++)
                {
                    if (enemyMgr.enemycreateInfos[i].type == EnemyType.scorpion)
                    {
                        enemyMgr.enemycreateInfos[i].Weight = 8000;
                        enemyMgr.enemycreateInfos[i].Consume = 1;
                        break;
                    }
                }
                
            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                
                for (int i = 0; i < grids.Length; i++)
                {
                    grids[i].gridType = GridType.Stone;
                }
                WorldManager.Instance.UnAbleLoad();
                WorldManager.Instance.SaveAndLoadName = "Level1_5_Hell";
                WorldManager.Instance.Load();
                EnemyManager.Instance.EnemySpeedAdder = 0.5f;
            }

            EventMgr.Instance.AddEventListener("GameWin", WhenGameWin);

        }

        public void WhenGameStart()
        {

           

        }
        public void WhenGameWin()
        {
            MySystem.Instance.nowUserData.AddstoreItem("Ë¿Ïß");
            MySystem.Instance.nowUserData.AddstoreItem("¼ý");

            MySystem.Instance.SaveNowUserData();
    
        }





    }
}