using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLevelEvent : BaseManager<BaseLevelEvent>
{
    public bool IsConveyor;
    public bool ScorpionAttackSpeedUp;//蝎子远程攻击频率加剧
    public bool SworderCrazyMode;
    public bool EnemyChaoes;
    public bool PlayerAttackHurt;
    public bool BloodZombieMoveFastWhenHurt;
    public bool PlayerDisappear;
    public bool MonsterNotFallItem;
    public bool HidePhoneButton;
    public bool SunMore;

    public void ShowDave(string[] texts)
    {
        if (!(GameStateMgr.Instance.EnterGameDirectly || MySystem.CanLoadLevelData()))
        {
            DaveTalkManager.Instance.ShowDave();
            MonoController.Instance.Invoke(0.7f, () => DaveTalkManager.Instance.ShowTalkWindow(texts ));
          
        }
    }
    protected void GetPlantWhenWin(PlantsType type)
    {
        EventMgr.Instance.AddEventListener("GameWin", () =>
        {
            if (MySystem.Instance.nowUserData.AddPlantInUserData(type))
                WhenGetPlantPannel.Instance.Show(type);
            MySystem.Instance.SaveNowUserData();
        });
    }
    public degreetype GetLevelType()
    {
        if (MySystem.Instance == null) return degreetype.normal;
       return MySystem.Instance.nowUserData.LevelType;
    }
    public void AddStoreItem(string objectName)
    {
        EventMgr.Instance.AddEventListener("GameWin", () =>
        {
            MySystem.Instance.nowUserData.AddstoreItem(objectName);
             MySystem.Instance.SaveNowUserData();
        });

        
    }
}
