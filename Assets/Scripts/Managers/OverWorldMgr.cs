using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OverWorld
{


    public class OverWorldMgr : BaseManager<OverWorldMgr>
    {
        public Vector3 RestartPos;
        private void Start()
        {
            UserData data = MySystem.getUserData();
            MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.peaShooter);
            MySystem.Instance.SaveNowUserData();
            Invoke("LateInvoke", 0.01f);
            MySystem.Instance.whenSaveAction += Save;
            //如果仍有未加载的游戏
            if (MySystem.Instance.nowLeveldata != null && MySystem.Instance.nowLeveldata.canLoad == true)
            {
               // Debug.Log(MySystem.Instance.nowUserData.sceneName);
                if( MySystem.Instance.nowUserData.sceneName != null &&MySystem.Instance.nowUserData.sceneName != "")
                    SceneMgr.Instance.LoadAsync(MySystem.Instance.nowUserData.sceneName);
            }
            string sceneName = SceneMgr.GetSceneNameStatic();
            for (int i = 0; i < data.overWorldInfos.Count; i++)
            {
                if (data.overWorldInfos[i].SceneName == sceneName)
                {
                    Load(data.overWorldInfos[i]);
                }
            }
            
        }
        private void LateInvoke()
        { 
            EventMgr.Instance.EventTrigger("OnRCardQuit");
        }
        public void Load(OverWorldInfo overWorldinfo)
        {

            if (overWorldinfo != null && overWorldinfo.loaded == true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                OverWorldInfo info = overWorldinfo;
                Transform playerTransform = player.transform;
                
                
                playerTransform.position = info.PlayerPosition;
                
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                PlayerHunger phu = player.GetComponent<PlayerHunger>();
                ph.Health = info.PlayerHealth;
              
                ph.ReFresh();
                phu.Hunger = info.PlayerHunger;
                phu.Hunger_over = info.PlayerHunger_over;
                phu.onHungerChange?.Invoke();
                GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Blocks");
                for (int i = 0; i < allBlocks.Length; i++)
                {
                    Destroy(allBlocks[i]);
                }
                for (int i = 0; i < info.BlocksPosition.Length; i++)
                {
                  GameObject g = Instantiate(ResourceSystem.Instance.GetBlock(info.BlocksType[i]).prefab,transform);
                  g.transform.position = info.BlocksPosition[i];
                }
                
            }

        }
       
        public void Save()
        {
            OverWorldInfo go = new OverWorldInfo();
            go.SceneName = SceneMgr.Instance.GetSceneName();
            go.PlayerPosition = PlayerHealth.instance.transform.position;
            go.PlayerHealth = PlayerHealth.instance.Health;
            go.PlayerHunger = PlayerHunger.Instance.Hunger;
            go.PlayerHunger_over = PlayerHunger.Instance.Hunger_over;
       
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Blocks");
            FunctionalBlock[] blocks = new FunctionalBlock[allBlocks.Length];
            go.BlocksPosition = new Vector3[blocks.Length];
            go.BlocksType = new FunctionalBlockType[blocks.Length];
            go.loaded = true;
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = allBlocks[i].GetComponent<FunctionalBlock>();
                go.BlocksPosition[i] = blocks[i].transform.position;
                go.BlocksType[i] = blocks[i].Btype;
            }
            MySystem.Instance.nowUserData.SaveOverWorldInfo(go);
        }
    }
}
[System.Serializable]
public class OverWorldInfo
{
    public string SceneName;
    public bool loaded;
    public Vector3 PlayerPosition;
    public float playerMaxHeath;
    public float PlayerHealth;
    public int PlayerHunger;
    public int PlayerHunger_over;
    public Vector3[] BlocksPosition;
    public FunctionalBlockType[] BlocksType;

}