using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelSelectPannel : Pannel
{
    public Sprite[] startButtonSprites;
    [HideInInspector] public Image startButtonImage;
    [HideInInspector] public Button StartGameButton;
    [HideInInspector] public Button normalButton;
    [HideInInspector] public Button hardButton;
    [HideInInspector] public Button hellButton;
    public Slot[] normalItemSlots;
    public Slot[] hardItemsSlots;
    public Slot[] hellItemsSlots;
    private Scriptable_Levelinfo_Single targetLevel;
    private degreetype leveltype;
    public Text LevelNameText;
    public TextPrinter LevelDescrptionPrinter;
    public GameObject[] locks;
    public GameObject[] UI_HardMode;
    public GameObject[] UI_HellMode;

    public override void Init()
    {
        base.Init();
        gameObject.SetActive(false);

    }
    //设置当前选着的LEVEL，并更新显示，更新按钮的功能
    public void SetLevel(Scriptable_Levelinfo_Single levelinfo)
    {
        if (targetLevel == null || targetLevel.LevelName != levelinfo.LevelName)
        {
            targetLevel = levelinfo;
            leveltype = degreetype.normal;
            UpdateOnce();

            // Debug.Log(targetLevel.LevelName);
            normalButton.onClick.RemoveAllListeners();

            normalButton.onClick.AddListener(() => leveltype = degreetype.normal);
            normalButton.onClick.AddListener(UpdateOnce);

            hardButton.onClick.RemoveAllListeners();
            hardButton.onClick.AddListener(() => leveltype = degreetype.hard);
            hardButton.onClick.AddListener(UpdateOnce);
            //if (MySystem.Instance.nowUserData.GetLevelFinishData(targetLevel.LevelName, degreetype.normal))
            //{
            //    hardButton.onClick.AddListener(() => leveltype = degreetype.hard);
            //    hardButton.onClick.AddListener(UpdateOnce);
            //}
            //else
            //{
            //        hardButton.onClick.AddListener(() => InfoWindow.Instance.Show("提示", "你需要先完成普通难度"));
            //}
            hellButton.onClick.RemoveAllListeners();
            hellButton.onClick.AddListener(() => leveltype = degreetype.hell);
            hellButton.onClick.AddListener(UpdateOnce);
            //if (MySystem.Instance.nowUserData.GetLevelFinishData(targetLevel.LevelName, degreetype.hard))
            //{
            //    hellButton.onClick.AddListener(() => leveltype = degreetype.hell);
            //    hellButton.onClick.AddListener(UpdateOnce);
            //}
            //else
            //{
            //        hellButton.onClick.AddListener(() => InfoWindow.Instance.Show("提示", "你需要先完成困难难度"));
            //}


        }
    }
    //更新信息
    private void UpdateOnce()
    {
        if (targetLevel != null)
        {
            for (int i = 0; i < UI_HardMode.Length; i++)
            {
                UI_HardMode[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < UI_HellMode.Length; i++)
            {
                UI_HellMode[i].gameObject.SetActive(false);
            }

            if (targetLevel.ExLevelNum > 0)
            {
                for (int i = 0; i < UI_HardMode.Length; i++)
                {
                    UI_HardMode[i].gameObject.SetActive(true);
                }
            }
            if (targetLevel.ExLevelNum > 1)
            {
                for (int i = 0; i < UI_HellMode.Length; i++)
                {
                    UI_HellMode[i].gameObject.SetActive(true);
                }
            }
            StartGameButton.onClick.RemoveAllListeners();
            LevelNameText.text = targetLevel.LevelName;
            ItemShow();
            //if (MySystem.Instance.nowUserData.GetLevelFinishData(targetLevel.LevelName, degreetype.normal)) locks[0].SetActive(false); else locks[0].SetActive(true);
            //if (MySystem.Instance.nowUserData.GetLevelFinishData(targetLevel.LevelName, degreetype.hard)) locks[1].SetActive(false); else locks[1].SetActive(true);
            switch (leveltype)
            {
                case degreetype.normal:
                    startButtonImage.sprite = startButtonSprites[0];
                    StartGameButton.onClick.AddListener(() => SelectWindow.Instance.Show("是否开始游戏", () => LoadLevel(targetLevel, degreetype.normal)));

                    //设置描述
                    LevelDescrptionPrinter.TextIndexReset();

                    LevelDescrptionPrinter.Sentences[0] = targetLevel.Describe;
                    LevelDescrptionPrinter.StartPrintNextSentence();


                    break;
                case degreetype.hard:
                    startButtonImage.sprite = startButtonSprites[1];
                    StartGameButton.onClick.AddListener(() => SelectWindow.Instance.Show("是否开始游戏(困难)", () => LoadLevel(targetLevel, degreetype.hard)));

                    LevelDescrptionPrinter.TextIndexReset();
                    LevelDescrptionPrinter.Sentences[0] = targetLevel.Describe_hard_ex;
                    LevelDescrptionPrinter.StartPrintNextSentence();

                    break;
                case degreetype.hell:
                    startButtonImage.sprite = startButtonSprites[2];
                    StartGameButton.onClick.AddListener(() => SelectWindow.Instance.Show("是否开始游戏(地狱)", () => LoadLevel(targetLevel, degreetype.hell)));

                    LevelDescrptionPrinter.TextIndexReset();
                    LevelDescrptionPrinter.Sentences[0] = targetLevel.Describe_hell_ex;
                    LevelDescrptionPrinter.StartPrintNextSentence();
                    break;
            }
        }

    }
    private void ItemShow()
    {
        //设置ITEM显示普通
        for (int i = 0; i < normalItemSlots.Length; i++)
        {
            normalItemSlots[i].CleanItem();
        }
        ItemInfo[] levelItem = targetLevel.GetItem(degreetype.normal);
        for (int i = 0; i < normalItemSlots.Length && i < levelItem.Length; i++)
        {
            normalItemSlots[i].info = levelItem[i].ShallowClone();
            normalItemSlots[i].ItemUpdate();
        }
        //设置ITEM显示困难
        for (int i = 0; i < hardItemsSlots.Length; i++)
        {
            hardItemsSlots[i].CleanItem();
        }
        ItemInfo[] levelItem2 = targetLevel.GetItem(degreetype.hard);
        for (int i = 0; i < hardItemsSlots.Length && i < levelItem2.Length; i++)
        {
            hardItemsSlots[i].info = levelItem2[i].ShallowClone();
            hardItemsSlots[i].ItemUpdate();
        }
        //设置ITEM显示地狱
        for (int i = 0; i < hellItemsSlots.Length; i++)
        {
            hellItemsSlots[i].CleanItem();
        }
        ItemInfo[] levelItem3 = targetLevel.GetItem(degreetype.hell);
        for (int i = 0; i < hellItemsSlots.Length && i < levelItem3.Length; i++)
        {
            hellItemsSlots[i].info = levelItem3[i].ShallowClone();
            hellItemsSlots[i].ItemUpdate();
        }
    }
    //按钮按下时执行
    private void LoadLevel(Scriptable_Levelinfo_Single targetLevel, degreetype type)
    {
        MySystem.Instance.nowUserData.LevelType = type;
        MySystem.Instance.nowUserData.levelName = targetLevel.LevelName;
        
        switch (type)
        {
            case degreetype.normal:

                MySystem.Instance.nowUserData.sceneName = targetLevel.LevelSceneName;
                SceneMgr.Instance.LoadAsync(targetLevel.LevelSceneName);

                break;
            case degreetype.hard:
                MySystem.Instance.nowUserData.sceneName = targetLevel.LevelSceneName_hard;
                SceneMgr.Instance.LoadAsync(targetLevel.LevelSceneName_hard);
                break;
            case degreetype.hell:
                MySystem.Instance.nowUserData.sceneName = targetLevel.LevelSceneName_hell;
                SceneMgr.Instance.LoadAsync(targetLevel.LevelSceneName_hell);
                break;
        }
        MySystem.Instance.SaveNowUserData();
    }

}
