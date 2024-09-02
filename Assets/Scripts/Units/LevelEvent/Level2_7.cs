using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    
    public class Level2_7 : BaseLevelEvent
    {
         
        private void Start()
        {
            if (MySystem.getUserData().LevelType == degreetype.hard)
            {
                EnemyManager.Instance.k *= 1.5f;
            }
        }
    }

}
