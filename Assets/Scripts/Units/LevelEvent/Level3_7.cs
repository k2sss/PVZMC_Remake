using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{
    public class Level3_7 : BaseLevelEvent
    {
        private void Start()
        {
            SunManager.Instance.FallSun = false;
            ShowDave(new string[]
            {
                "�ڣ���ڣ���TM��",
                "�������Щөʯ������Ȼ������ͳ�Ϲ����",
                
            });
        }
    }
}

