using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SettingUI
{
    public class E_Lable : SettingElement
    {
        public override void Init(string LableName)
        {
            this.LableName = LableName;
            gameObject.GetComponent<Text>().text = LableName;
        }

        public override void Init(string LableName, Color color)
        {
            this.LableName = LableName;
            gameObject.GetComponent<Text>().text = LableName;
            gameObject.GetComponent<Text>().color = color;
        }
        public override void Init(string LableName, Color color, string SaveKey, float InitValue, float StartIndex = 0, float Multiplier = 100)
        {
           
        }

        public override void Save()
        {
            
        }
    }

}
