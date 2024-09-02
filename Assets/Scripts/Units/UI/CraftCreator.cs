using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace K2STools
{
    public class CraftCreator : MonoBehaviour
    {
        [SerializeField] private string CraftName = "默认";
        [SerializeField] private int[] itemSources = new int[9];
        [SerializeField] private int targetItemType;
        [SerializeField] private int targetItemNum = 1;
        private CraftManager manager;
        private Queue<CraftInfo> stepInfos = new Queue<CraftInfo>();

        public void Create()
        {
            if (manager == null)
                manager = GameObject.Find("Systems/CraftMgr").GetComponent<CraftManager>();

            if (itemSources.Length != 9)
                return;
            CraftInfo info = new CraftInfo(CraftName, itemSources, targetItemType, targetItemNum);

            manager.CraftInfos.Add(info);

            stepInfos.Enqueue(info);
            Log();

            Debug.Log("请OverRide System/CraftMgr");
            //log         
        }

        public void Log()
        {


            for (int i = 0; i < itemSources.Length; i++)
                Debug.Log("原料:" + (ItemType)itemSources[i]);

            Debug.Log("输出" + (ItemType)targetItemType + "*" + targetItemNum);


        }
        public void Back()
        {
            if (manager == null)
                manager = GameObject.Find("Systems/CraftMgr").GetComponent<CraftManager>();

            manager.CraftInfos.Remove(stepInfos.Dequeue());
           
            Debug.Log("撤销成功");
            
        }
    }



}

