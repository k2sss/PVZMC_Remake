using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace SettingUI
{
    public class E_Button : SettingElement
    {
        private int buttonCount;
        private int nowSelectIndex = 0;
        private List<Action> buttonActionsList = new List<Action>();
        private List<Image> buttonimgList = new List<Image>();
        public void Init(string LableName, Color color,string savekey,string[] buttonLables, Action[] buttonActions)
        {
            if (buttonLables.Length != buttonActions.Length)
                return;
            this.saveKey = savekey;
            buttonCount = buttonLables.Length;
            GameObject smallButton = FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/Button");
            Transform smallButtonParent = transform.GetChild(0).transform;
            for (int i = 0; i < buttonLables.Length; i++)
            {
                int newi = i;
                GameObject g = Instantiate(smallButton, smallButtonParent);
                Button b = g.GetComponent<Button>();
                
               
                
                buttonActionsList.Add(() => buttonActions[newi]());
                buttonimgList.Add(g.GetComponent<Image>());
                g.transform.GetChild(0).GetComponent<Text>().text = buttonLables[i];
                b.onClick.AddListener(()=>Do(newi));
            }
            gameObject.GetComponent<Text>().text = LableName;
            gameObject.GetComponent<Text>().color = color;

        }
        public void Load()
        {
            if (PlayerPrefs.HasKey(saveKey))
            {
                int index = PlayerPrefs.GetInt(saveKey);
                Do(index);
            }
        }
        public void Do(int index)
        {
            //∏ﬂ¡¡
            for (int i = 0; i < buttonimgList.Count; i++)
            {
                if(i == index)
                buttonimgList[i].color = Color.yellow;
                else
                buttonimgList[i].color = Color.white;
            }

            if (index < buttonCount)
            {
                nowSelectIndex = index;
                buttonActionsList[index]?.Invoke();
            }
        }
        public override void Init(string LableName)
        {
            
        }

        public override void Init(string LableName, Color color)
        {
            
        }

        public override void Init(string LableName, Color color, string SaveKey, float InitValue, float StartIndex = 0, float Multiplier = 100)
        {
            
        }

        public override void Save()
        {
            PlayerPrefs.SetInt(saveKey, nowSelectIndex);
        }
    }
}

