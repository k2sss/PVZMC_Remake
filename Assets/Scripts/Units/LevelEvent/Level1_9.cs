using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level1_9 : BaseLevelEvent
    {
        // Start is called before the first frame update
        void Start()
        {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                EnemyManager.Instance.EnemyHealthMultiplier = 1.3f;
            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                PlantManager.Instance.AllPlantsHPtoOne = true;
            }

        }



    }
}