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
            "��ʲô�Ƶط�",
            "�ҵ�����Ũ���Ѫ��ζ��Ӱ���ҳ����׾��θ��" });

            GetPlantWhenWin(PlantsType.flowerPot);

            AddStoreItem("�̱�ʯ3");

        }  
       
    }


}
