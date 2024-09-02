using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace SettingUI
{
    public class SettingPage : MonoBehaviour
    {
        public string PageName { private set; get; }
        public Transform TargetContentTransform { private set; get; }
        public Sprite[] buttonSprites;
        private List<SettingElement> elements = new List<SettingElement>();
        private Image buttonImage;
        public void Init(string Name,Transform targetContent)
        {
            PageName = Name;
            TargetContentTransform = targetContent;
            buttonImage = GetComponent<Image>();
        }
        public void SavePage()
        {
            foreach (var element in elements)
            {
                element.Save();
            }
            PlayerPrefs.Save();
        }
        public void Show()
        {
            TargetContentTransform.gameObject.SetActive(true);
            buttonImage.sprite = buttonSprites[1];
        }
        public void Close()
        {
            TargetContentTransform.gameObject.SetActive(false);
            buttonImage.sprite = buttonSprites[0];
        }
        public void AddLable(string ElementName)
        {
            GameObject lablePrefab = FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/Lable");
            SettingElement el = Instantiate(lablePrefab, TargetContentTransform).GetComponent<SettingElement>();
            elements.Add(el);
            el.Init(ElementName);
        }
        public void AddLable(string ElementName,Color color)
        {

            GameObject lablePrefab = FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/Lable");
            SettingElement el = Instantiate(lablePrefab, TargetContentTransform).GetComponent<SettingElement>();
            elements.Add(el);
            el.Init(ElementName,color);
        }
        public void AddSmallLable(string ElementName, Color color)
        {
            GameObject lablePrefab = FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/Lable_small");
            SettingElement el = Instantiate(lablePrefab, TargetContentTransform).GetComponent<SettingElement>();
            elements.Add(el);
            el.Init(ElementName, color);
        }

        public E_Slider AddSlider(string ElementName, Color color,string saveKey,float initValue)//Ìí¼ÓÔªËØ
        {
            GameObject sliderPrefab = FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/Slider");
            initValue = Mathf.Clamp(initValue,0,1f);
            SettingElement el = Instantiate(sliderPrefab, TargetContentTransform).GetComponent<SettingElement>();
            elements.Add(el);
           
            el.Init(ElementName, color,saveKey,initValue);

            return el as E_Slider;
        }
        public E_Button AddButton(string LableName, Color color,string savekey, string[] buttonLables, Action[] buttonActions,int defaultValue)
        {
            GameObject GroupPrefab = FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/ButtonGroup");
            E_Button go = Instantiate(GroupPrefab, TargetContentTransform).GetComponent<E_Button>();
            elements.Add(go);
            
            go.Init(LableName,color,savekey,buttonLables,buttonActions);
            go.Do(defaultValue);
            go.Load();
            return go;
        }
    }
}
