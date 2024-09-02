using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
//using UnityEditor.Rendering.Universal;
namespace SettingUI
{
    public class SettingPannel : Pannel
    {
        [HideInInspector] public Transform PageButtonParent;
        [HideInInspector] public Transform PageParent;
        public Button BackButton;
        public List<SettingPage> pages = new List<SettingPage>();
        public UniversalRendererData URD;
        public UniversalRenderPipelineAsset URPA;
        public UniversalRenderPipelineAsset URPA_Blanced;

        private void Start()
        {
            //创建页面
            CreateANewPage("游戏设置");
            CreateANewPage("音效设置");
            CreateANewPage("画质设置");
            //创建标签
            pages[0].AddLable("皮肤路径：", Color.yellow);
            string SkinPath;
            if (PhoneControlMgr.PhoneControl == false)
                SkinPath = Application.dataPath + "/skin/steve.png";
            else
                SkinPath = Application.persistentDataPath + "/skin/steve.png";
            pages[0].AddSmallLable($"{SkinPath}", Color.white);
            pages[0].AddSmallLable($"皮肤安装教程：在上述路径中放置皮肤文件，并改名为steve.png", Color.yellow);
            pages[0].AddButton("角色类型", Color.white, "characterType", new string[2] { "Steve", "Alex" }, new Action[2]
            {
                 ()=>
                 {   GameObject g = GameObject.FindGameObjectWithTag("Player");
                     if(g!=null)
                     g.GetComponent<SkinLoader>().ChangePlayerSlim(0);
                 } ,
                  ()=>
                 {     GameObject g = GameObject.FindGameObjectWithTag("Player");
                     if(g!=null)
                     g.GetComponent<SkinLoader>().ChangePlayerSlim(1);
                 } 
            },0);
            pages[0].AddButton("怪物显血", Color.white, "isShowHP", new string[2] { "开", "关" }, new Action[2]
            {
                ()=>{if(HpBarMgr.Instance != null)HpBarMgr.Instance.isEnable = true; },
                ()=>{if(HpBarMgr.Instance != null)HpBarMgr.Instance.isEnable = false;}
    },0);
            pages[0].AddButton("伤害飘字", Color.white, "isShowDamage", new string[2] { "开", "关" }, new Action[2]
             {
                ()=> {if(DamageNumMgr.Instance != null)DamageNumMgr.Instance.isEnable = true; },
                ()=> {if(DamageNumMgr.Instance != null)DamageNumMgr.Instance.isEnable = false; }
             }, 0);



            AudioMixer mixer = SoundSystem.Instance.SoundMixer.audioMixer;
            E_Slider slider_audio_main = pages[1].AddSlider("总音量", Color.white, "MainAudio", 1f);
            E_Slider slider_audio = pages[1].AddSlider("音效大小", Color.white, "Audio", 1f);
            E_Slider slider_music = pages[1].AddSlider("音乐大小", Color.white, "Music", 1f);
           
            slider_audio_main.AddOnValueChangeListener(() =>
            {
                int clamped = (int)(20 * Mathf.Log10(Mathf.Clamp(slider_audio_main.thisSlider.value, 0.0001f, 1)));
                mixer.SetFloat("MainValue", clamped);
            }
            );
            slider_audio.AddOnValueChangeListener(() =>
            {
                int clamped = (int)(20 * Mathf.Log10(Mathf.Clamp(slider_audio.thisSlider.value, 0.0001f, 1)));
                mixer.SetFloat("SoundValue", clamped);
            });
            slider_music.AddOnValueChangeListener(() =>
            {
                int clamped = (int)(20 * Mathf.Log10(Mathf.Clamp(slider_music.thisSlider.value, 0.0001f, 1)));
                mixer.SetFloat("MusicValue", clamped);
            });
          
            pages[2].AddButton("阴影距离", Color.white, "shadowDistance",
                 new string[3] { "低", "中", "高" },
                 new Action[3] { () => ChangeShadowQuality(0), () => ChangeShadowQuality(1), () => ChangeShadowQuality(2) }, 2);
            pages[2].AddButton("全屏化", Color.white, "Screen",
              new string[2] { "是", "否" },
              new Action[2] { () => Screen.fullScreen = true, () => Screen.fullScreen = false }, 0);
            pages[2].AddButton("雾", Color.white, "fog",
                new string[2] { "开", "关" },
                new Action[2] { () => RenderSettings.fog = true, () => RenderSettings.fog = false }, 0);

            E_Slider renderScaleSlider = pages[2].AddSlider("渲染分辨率", Color.white, "renderScale", 1f);
            


            renderScaleSlider.AddOnValueChangeListener(() =>
            {
                ChangeRenderScale(renderScaleSlider.thisSlider.value);
            });
           
            Last();
        }
        private void Last()
        {
            TurnToPage(pages[0]);
            BackButton.onClick.AddListener(() =>
            {
                SoundSystem.Instance.Play2Dsound("Click");
                PopUI();
            });
            //退出时保存
            UIMgr.Instance.AddPopAction(PannelName, () =>
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    pages[i].SavePage();
                }
            });
            gameObject.SetActive(false);

        }
        public void CreateANewPage(string PageName)
        {
            //生成page按钮
            GameObject page = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/PageButton"), PageButtonParent);
            Button pageButton = page.GetComponent<Button>();
            SettingPage spage = page.GetComponent<SettingPage>();
            Text pageButtonText = page.transform.GetChild(0).gameObject.GetComponent<Text>();
            pageButtonText.text = PageName;
            //生成ContentParent
            GameObject pageContent = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/PageContent"), PageParent); ;
            Transform contentParent = pageContent.transform.GetChild(0).transform.GetChild(0).transform;
            contentParent.name = PageName;
            contentParent.gameObject.SetActive(false);//默认显示关闭
            spage.Init(PageName, contentParent);
            pages.Add(spage);
            pageButton.onClick.AddListener(
                () =>
                {
                    SoundSystem.Instance.Play2Dsound("Click");
                    TurnToPage(spage);
                });

        }
        public void TurnToPage(SettingPage page)
        {
            for (int i = 0; i < pages.Count; i++)
            {

                if (pages[i] == page)
                {
                    pages[i].Show();//打开页面
                }
                else
                {

                    pages[i].Close();//关闭页面
                }
            }
        }
        private void ChangeShadowQuality(int quality)
        {
            if (quality == 0)
            {
                URPA.shadowDistance = 5;
                URPA_Blanced.shadowDistance = 0;
            }
            if (quality == 1)
            {
                URPA.shadowDistance = 15;
                URPA_Blanced.shadowDistance = 12;
            }
            if (quality == 2)
            {
                URPA.shadowDistance = 40;
                URPA_Blanced.shadowDistance = 30;
            }
        }
        private void ChangeRenderScale(float renderScale)
        {
            renderScale = Mathf.Clamp(renderScale, 0.1f, 1);
            URPA.renderScale = renderScale;
            URPA_Blanced.renderScale = renderScale;
        }

    }
}