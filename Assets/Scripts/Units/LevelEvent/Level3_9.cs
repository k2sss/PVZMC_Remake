using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{
    public class Level3_9 : BaseLevelEvent
    {
        // Start is called before the first frame update
        void Start()
        {
            SunManager.Instance.FallSun = false;
            ShowDave(new string[]
            {
                "��������㣡��",
                "���Դ���Ķ����о�ʮ��Σ��!"

            });
            AddStoreItem("�ɺ춧");
        }

        
    }

}
