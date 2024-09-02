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
                "发光大章鱼！？",
                "它脑袋里的东西感觉十分危险!"

            });
            AddStoreItem("猩红锭");
        }

        
    }

}
