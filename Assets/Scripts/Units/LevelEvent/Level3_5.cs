using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{
    public class Level3_5 : BaseLevelEvent
    {

        private void Start()
        {
            AddStoreItem("��������");

            degreetype type = GetLevelType();

            if (type == degreetype.hard)
            {
                PlayerItemManager.Instance.IsAble = false;
            }
        }
    }
}

