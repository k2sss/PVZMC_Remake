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
                "黑，真黑，真TM黑",
                "多亏了那些萤石树，不然我们早就成瞎子了",
                
            });
        }
    }
}

