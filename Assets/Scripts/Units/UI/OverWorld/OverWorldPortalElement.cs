using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace OverWorld
{
    public class OverWorldPortalElement : MonoBehaviour
    {
        private InGameMenuInfo info;
        public void Init(InGameMenuInfo info)
        {
            this.info = info;
            transform.GetChild(0).GetComponent<Text>().text = info.WorldName;
            transform.GetChild(1).GetComponent<Image>().sprite = info.WorldSprite;
            transform.GetChild(2).GetComponent<Text>().text = info.WorldDescrption;
        }
        public void ButtonDown()
        {
            if (info != null)
            {
                MySystem.Instance.nowUserData.NowInGameMenuSceneName = info.SceneName;
                MySystem.Instance.SaveNowUserData();
             
                SceneMgr.Instance.LoadAsync(info.SceneName);
            }
        }
    }

}