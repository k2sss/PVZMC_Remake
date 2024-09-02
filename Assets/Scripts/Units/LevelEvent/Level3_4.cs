using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3_4 : BaseLevelEvent
{
    public PlantManager plantManager;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        degreetype currentLevelType = MySystem.Instance.nowUserData.LevelType;
        if (currentLevelType == degreetype.normal)
        {
            ShowDave(new string[] {
            "������һ�����˵ļһ�",
            "�����ڿ��й������ǵ�ֲ��",
            "��ʲô�����Ը���?"
            });
            return;
        }
        if (currentLevelType == degreetype.hard)
        {
            
            if (plantManager == null) return;
            plantManager.isRandomPlantType = true;
            plantManager.IsPlantTypeProbability = 70;
            if (CardSlot.Instance == null) return;
            CardSlot.Instance.SetSunCount(500);
            return;
        }



    }

}
