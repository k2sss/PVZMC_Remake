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
            //����ҳ��
            CreateANewPage("��Ϸ����");
            CreateANewPage("��Ч����");
            CreateANewPage("��������");
            //������ǩ
            pages[0].AddLable("Ƥ��·����", Color.yellow);
            string SkinPath;
            if (PhoneControlMgr.PhoneControl == false)
                SkinPath = Application.dataPath + "/skin/steve.png";
            else
                SkinPath = Application.persistentDataPath + "/skin/steve.png";
            pages[0].AddSmallLable($"{SkinPath}", Color.white);
            pages[0].AddSmallLable($"Ƥ����װ�̳̣�������·���з���Ƥ���ļ���������Ϊsteve.png", Color.yellow);
            pages[0].AddButton("��ɫ����", Color.white, "characterType", new string[2] { "Steve", "Alex" }, new Action[2]
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
            pages[0].AddButton("������Ѫ", Color.white, "isShowHP", new string[2] { "��", "��" }, new Action[2]
            {
                ()=>{if(HpBarMgr.Instance != null)HpBarMgr.Instance.isEnable = true; },
                ()=>{if(HpBarMgr.Instance != null)HpBarMgr.Instance.isEnable = false;}
    },0);
            pages[0].AddButton("�˺�Ʈ��", Color.white, "isShowDamage", new string[2] { "��", "��" }, new Action[2]
             {
                ()=> {if(DamageNumMgr.Instance != null)DamageNumMgr.Instance.isEnable = true; },
                ()=> {if(DamageNumMgr.Instance != null)DamageNumMgr.Instance.isEnable = false; }
             }, 0);



            AudioMixer mixer = SoundSystem.Instance.SoundMixer.audioMixer;
            E_Slider slider_audio_main = pages[1].AddSlider("������", Color.white, "MainAudio", 1f);
            E_Slider slider_audio = pages[1].AddSlider("��Ч��С", Color.white, "Audio", 1f);
            E_Slider slider_music = pages[1].AddSlider("���ִ�С", Color.white, "Music", 1f);
           
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
          
            pages[2].AddButton("��Ӱ����", Color.white, "shadowDistance",
                 new string[3] { "��", "��", "��" },
                 new Action[3] { () => ChangeShadowQuality(0), () => ChangeShadowQuality(1), () => ChangeShadowQuality(2) }, 2);
            pages[2].AddButton("ȫ����", Color.white, "Screen",
              new string[2] { "��", "��" },
              new Action[2] { () => Screen.fullScreen = true, () => Screen.fullScreen = false }, 0);
            pages[2].AddButton("��", Color.white, "fog",
                new string[2] { "��", "��" },
                new Action[2] { () => RenderSettings.fog = true, () => RenderSettings.fog = false }, 0);

            E_Slider renderScaleSlider = pages[2].AddSlider("��Ⱦ�ֱ���", Color.white, "renderScale", 1f);
            


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
            //�˳�ʱ����
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
            //����page��ť
            GameObject page = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/PageButton"), PageButtonParent);
            Button pageButton = page.GetComponent<Button>();
            SettingPage spage = page.GetComponent<SettingPage>();
            Text pageButtonText = page.transform.GetChild(0).gameObject.GetComponent<Text>();
            pageButtonText.text = PageName;
            //����ContentParent
            GameObject pageContent = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("UI/SettingUI/PageContent"), PageParent); ;
            Transform contentParent = pageContent.transform.GetChild(0).transform.GetChild(0).transform;
            contentParent.name = PageName;
            contentParent.gameObject.SetActive(false);//Ĭ����ʾ�ر�
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
                    pages[i].Show();//��ҳ��
                }
                else
                {

                    pages[i].Close();//�ر�ҳ��
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