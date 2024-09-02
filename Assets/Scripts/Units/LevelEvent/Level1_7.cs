using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level1_7 : BaseLevelEvent
    {
        
        private void Start()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.normal)
            {

            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                SunMore = false;
            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                EnemyManager.Instance.b = 3;
                EnemyManager.Instance.k = 2;
                EnemyManager.Instance.EnemyAttackSpeedAdder = 0.5f;
                EnemyManager.Instance.EnemySpeedAdder = 0.3f;
            }
        }
          


    }
}