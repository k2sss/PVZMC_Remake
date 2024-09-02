using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace SettingUI
{
    public class E_Slider : SettingElement
    {
        public Slider thisSlider { get; private set; }
        public Text targetValueText;
        public override void Init(string LableName)
        {
           
        }

        public override void Init(string LableName, Color color)
        {
           
        }
        public override void Init(string LableName, Color color, string SaveKey, float InitValue, float StartIndex = 0, float Multiplier = 100)
        {
            thisSlider = transform.GetChild(0).gameObject.GetComponent<Slider>();
            thisSlider.value = InitValue;//设置初始化的值

            this.LableName = LableName;
            this.saveKey = SaveKey;
            gameObject.GetComponent<Text>().text = LableName;
            gameObject.GetComponent<Text>().color = color;
            //Load
            if (PlayerPrefs.HasKey(saveKey))
            {
                thisSlider.value = PlayerPrefs.GetFloat(saveKey);
            }
            
            thisSlider.onValueChanged.AddListener((p) =>
            {
                int show = (int)(StartIndex + thisSlider.value * Multiplier);
                targetValueText.text = show.ToString();
            });
            //延迟触发
           
            MonoController.Instance.InvokeUnScaled(0.001f,()=>thisSlider.onValueChanged?.Invoke(0));
        }
        public void AddOnValueChangeListener(Action action)
        {
            thisSlider.onValueChanged.AddListener((p) =>action());
        }
        public override void Save()
        {
            PlayerPrefs.SetFloat(saveKey, thisSlider.value);
        }
       
    }

}