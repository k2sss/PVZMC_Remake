using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{
    public class Level3_10 : BaseLevelEvent
    {
        private void Start()
        {
            ShowDave(new string[]
            {
                "光！",
                "长时间呆在黑暗中真的容易让人发疯",
                "什么？我本来就是疯的！"
            });
            AddStoreItem("同志短剑");

            degreetype ltype = GetLevelType();
            if (ltype == degreetype.hard)
            {
                EnemyManager.Instance.EnemyAttackSpeedAdder = 2;
            }
        }

    }

}
