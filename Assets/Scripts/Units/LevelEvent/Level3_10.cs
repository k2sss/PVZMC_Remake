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
                "�⣡",
                "��ʱ����ںڰ�������������˷���",
                "ʲô���ұ������Ƿ�ģ�"
            });
            AddStoreItem("ͬ־�̽�");

            degreetype ltype = GetLevelType();
            if (ltype == degreetype.hard)
            {
                EnemyManager.Instance.EnemyAttackSpeedAdder = 2;
            }
        }

    }

}
