using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEvent
{
public class Level3_3 : BaseLevelEvent
   {
    
    void Start()
    {
            ShowDave(new string[2]
            {
            "水流！",
            "他能改变僵尸的朝向吗？"
            });
            AddStoreItem("爆桶G");
    }

}

}
