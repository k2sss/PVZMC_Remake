using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OverWorld
{
    public class Level : MonoBehaviour
    {
        private Transform PlayerTransform;
        private OverWorld.LevelManager manager;
        private Scriptable_Levelinfo_Single levelinfo;
        public float CheckRange = 4;
        public OverWorld.Level[] nextLevel;
        public bool isPlayerNearBy { get; private set;}
        public void Init()//被OverWorld.LevelManager统一调用
        {
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
           // Debug.Log(SelectlevelUI.name);
            //MonoController.Instance.HalfTickAction += ()=>SelectlevelUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if ((transform.position - PlayerTransform.position).magnitude < CheckRange)
            {
                isPlayerNearBy = true;
            }
            else
            {
                isPlayerNearBy = false;
            }
        }
        public void SetLevelManager(OverWorld.LevelManager mgr)
        {
            manager = mgr;
        }
        public void SetLevelInfo(Scriptable_Levelinfo_Single info)
        {
            levelinfo = info;
            if (MySystem.Instance.nowUserData.GetLevelFinishData(levelinfo.LevelName, degreetype.hell))
            {
                transform.GetChild(3).gameObject.SetActive(true);
            }
            else if(MySystem.Instance.nowUserData.GetLevelFinishData(levelinfo.LevelName, degreetype.hard))
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (MySystem.Instance.nowUserData.GetLevelFinishData(levelinfo.LevelName, degreetype.normal))
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }


        }
        public Scriptable_Levelinfo_Single GetLevelinfo()
        {
            if (levelinfo != null)
            {
                return levelinfo;
            }
            else
            {
                Debug.Log("无法获取关卡节点的level信息");
                return null;
            }
           
        }
    }
}

