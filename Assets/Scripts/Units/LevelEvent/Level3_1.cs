using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{
    public class Level3_1 : BaseLevelEvent
    {
        // Start is called before the first frame update
        void Start()
        {
          
            ShowDave(new string[2]
                    {
            "唔，什么破地方",
            "我当心这浓厚的血腥味会影响我吃玉米卷的胃口" });

            GetPlantWhenWin(PlantsType.flowerPot);

            AddStoreItem("绿宝石3");

        }  
       
    }


}
