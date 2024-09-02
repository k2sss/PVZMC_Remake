using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{


    public class Level2_4 : BaseLevelEvent
    {
        public GameObject blockParent;
        // Start is called before the first frame update
        void Start()
        {
           degreetype type = MySystem.Instance.nowLeveldata.LevelType;
            if (type == degreetype.hard)
            {
                blockParent.SetActive(true);
            }

        }
    }
}