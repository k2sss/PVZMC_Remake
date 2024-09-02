using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum degreetype { normal, hard, hell };
       
namespace OverWorld
{
    public class LevelManager : MonoBehaviour
    {
        public Scriptable_Levelinfo resources;
        private OverWorld.Level[] levels;
        private GameObject LevelUI;
        private LevelSelectPannel levelUIscr;
        private OverWorld.Level TargetLevel;
        public OverWorld.Level[] OriginLevel;//���
        private void Start()
        {
            LevelUI = UIMgr.Instance.GetUIObject("LevelSelect");
            levelUIscr = LevelUI.GetComponent<LevelSelectPannel>();
            InitSave();
            InitLevel(); 
            
        }
        private void Update()
        {
            if (canShowUI() == true)
            {
                LevelUI.SetActive(true);
                if(TargetLevel !=null)
                levelUIscr.SetLevel(TargetLevel.GetLevelinfo());
            }
            else
            {
                LevelUI.SetActive(false);
            }
        }
        public void InitLevel()
        {
            if (transform.childCount > resources.levels.Count)
            {
                Debug.LogWarning("�ؿ������ӹؿ�����ƥ��");
            }

            levels = new OverWorld.Level[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                levels[i] = transform.GetChild(i).gameObject.GetComponent<OverWorld.Level>();
                levels[i].SetLevelManager(this);
                levels[i].SetLevelInfo(resources.levels[i]);
                levels[i].Init();
                levels[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < OriginLevel.Length; i++)
            {
                OriginLevel[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < levels.Length; i++)
            {
                if(MySystem.Instance.nowUserData.GetLevelFinishData(levels[i].GetLevelinfo().LevelName))
                for (int j = 0; j < levels[i].nextLevel.Length; j++)
                {
                    levels[i].nextLevel[j].gameObject.SetActive(true);
                }
            }
        }//��ʼ���ӹؿ��ڵ㣬��ÿ���ؿ��ڵ㸳ֵScriptable_Levelinfo
        private bool canShowUI()
        {
            bool flag = false;
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i].isPlayerNearBy == true)
                {
                    flag = true;
                    TargetLevel = levels[i];
                    break;
                }
            }
            return flag;
        }//����ҿ����ӹؿ��ڵ�ʱΪtrue������Ϊfalse
        private void InitSave()//��ʼ���浵
        {
            for (int i = 0; i < resources.levels.Count; i++)
            {
                bool flag = false;
                for (int j = 0; j < MySystem.Instance.nowUserData._levelFinishData.Count; j++)
                {
                    if (resources.levels[i].LevelName == MySystem.Instance.nowUserData._levelFinishData[j].LevelName)
                    {
                        flag = true;
                        //flagΪtrue,��ʾ�浵�����иùؿ������
                        break;
                    }
                }
                if (flag == false)//���flag Ϊfalse,δ�иùؿ����ݣ����½�
                {
                    LevelFinishData newdata = new LevelFinishData();
                    newdata.LevelName = resources.levels[i].LevelName;
                    newdata.finishTheNormalLevel = newdata.finishTheHardLevel = newdata.finishTheHelllLevel = false;
                    MySystem.Instance.nowUserData._levelFinishData.Add(newdata);

                }
            }
            MySystem.Instance.SaveNowUserData();

        }

    }
}

