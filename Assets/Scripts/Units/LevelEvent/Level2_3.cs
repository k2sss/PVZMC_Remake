using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    
    public class Level2_3 : BaseLevelEvent
    {
        public GameObject sunLight;
        public PlantManager plantManager;
        private void Start()
        {
            if (MySystem.Instance.nowLeveldata.LevelType == degreetype.hard)
            {

            }
        }
    }
}