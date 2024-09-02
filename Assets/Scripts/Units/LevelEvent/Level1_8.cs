using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent { 
public class Level1_8 : BaseLevelEvent
{
    // Start is called before the first frame update
    void Start()
    {
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hard)
            {
                EnemyManager.Instance.EnemyAttackSpeedAdder = 0.6f;
            }
            if (MySystem.Instance.nowUserData.LevelType == degreetype.hell)
            {
                EnemyChaoes = true;
            }
        }


}
}