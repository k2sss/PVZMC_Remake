using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{
    public class Level1_4 : BaseLevelEvent
    {
        private float damageTimer;

        public EnemyManager enemyMgr;
        void Start()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.normal)
            {
                enemyMgr.EnemySpeedAdder = -0.2f;
                PlantManager.Instance.AttackSpeedAdder = +1f;
                PlantManager.Instance.DamageAdder = 0.2f;
                EventMgr.Instance.AddEventListener("GameStart", WhenGameStart);
            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                enemyMgr.EnemySpeedAdder = -0.2f;
                PlantManager.Instance.AttackSpeedAdder = 0.2f;
                PlantManager.Instance.DamageAdder = 0.2f;
                EventMgr.Instance.AddEventListener("GameStart", WhenGameStart);
            }
            else
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                enemyMgr.EnemySpeedAdder = -0.2f;
                PlantManager.Instance.AttackSpeedAdder = +1f;
                PlantManager.Instance.DamageAdder = 0.2f;
                EventMgr.Instance.AddEventListener("GameStart", WhenGameStart);
            }

            EventMgr.Instance.AddEventListener("GameWin", WhenGameWin);

        }
        private void Update()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer > 2)
                {
                    DamagePlants();
                    damageTimer = 0;
                }
            }
        }
        public void WhenGameStart()
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>().SandStormEffected = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>().WindStrength = -1.5f;

        }
        public void WhenGameWin()
        {
            if (MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.nuts))
                WhenGetPlantPannel.Instance.Show(PlantsType.nuts);
            MySystem.Instance.nowUserData.AddstoreItem("Ìú¶§");
            MySystem.Instance.SaveNowUserData();
        }
        public void DamagePlants()
        {
            GameObject[] plants = GameObject.FindGameObjectsWithTag("Plants");
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].GetComponent<Plants>().Hurt(1, 0, DamageType.trueDamage);
            }
        }
    }
}
