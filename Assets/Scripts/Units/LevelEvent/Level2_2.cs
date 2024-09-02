using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level2_2 : BaseLevelEvent
    {
        private void Start()
        {
            if (MySystem.Instance.nowLeveldata.LevelType == degreetype.hard)
            {
                BloodZombieMoveFastWhenHurt = true;
            }
        }

    }
}
