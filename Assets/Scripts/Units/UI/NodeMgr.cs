using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NodeMgr : MonoBehaviour
{
    [HideInInspector] public GameObject line;
     public Sprite[] BNodeButtonSprites;
    [HideInInspector] public Text nodeNameText;
    [HideInInspector] public TextPrinter nodeDescriptionTextPrinter;
    [HideInInspector] public BNODE targetNode;
    [HideInInspector] public Animator NodeWindowAnimator;
    [HideInInspector] public Image ButtonImage;
    [HideInInspector] public Text ButtonText;
    [HideInInspector] public Image ButtonGizmos;
    [HideInInspector] public Sprite[] lockSprites;
    public NeededSlotMgr slotMgr;
    private void Start()
    {
        UIMgr.Instance.AddPopAction("Basic", Save);
        Load();
        UpdateBNodeState();
    }
    [ContextMenu("标准化")]
    public void ReAdjust()
    {
        //连线
        DeleteLine();
        for (int i =0;i<transform.childCount;i++)
        {
            BNODE tar = transform.GetChild(i).gameObject.GetComponent<BNODE>();
            for(int n=0;n<tar.next.Length;n++)
            {
                GameObject g = Instantiate(line, tar.transform);
                RectTransform r = g.GetComponent<RectTransform>();
                Vector2 dir = tar.GetComponent<RectTransform>().anchoredPosition - tar.next[n].GetComponent<RectTransform>().anchoredPosition + new Vector2(50,0);
                float length = dir.magnitude;
                g.GetComponent<RectTransform>().sizeDelta = new Vector2(length, r.sizeDelta.y);
                g.transform.right = -dir;
                tar.next[n].targetLineImg = g.GetComponent<Image>();
            }

        }
        

    }

    [ContextMenu("删除线")]
    public void DeleteLine()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject c = transform.GetChild(i).gameObject;
            for(int j = 0;j<c.transform.childCount;j++)
            {
                if(c.transform.GetChild(j).name == "Line(Clone)")
                {
                    DestroyImmediate(c.transform.GetChild(j).gameObject);
                    j--;
                }

            }

        }
    }
    public void UnLock()
    {
        if(targetNode.stage != 2)
        {
            if (slotMgr.CanUnLock() == true)
            {
                SelectWindow.Instance.Show("你确认要升级吗?", () => UnLockStuff());
            }
            else
            {
                InfoWindow.Instance.Show("提示","材料不足");
            }
        }
    }
    public void UnLockStuff()
    {
        //WhenGetPlantPannel.Instance.Show(PlantsType.peaShooter);
        for (int i = 0; i < targetNode.neededItems.Length; i++)
        {
            InventoryManager.Instance.Deleteitems(targetNode.neededItems[i]);
        }
        switch(targetNode.NodeID)
        {
            case 1:
                MySystem.Instance.nowUserData.UnLockCraftTable = true;
                SceneObjMgr.Instance.UpdateState();
                break;
            case 2:
                MySystem.Instance.nowUserData.UnLockChest = true;
                SceneObjMgr.Instance.UpdateState();
                break;
            case 3:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.Cactus);
                WhenGetPlantPannel.Instance.Show(PlantsType.Cactus);
                break;
            case 4:
                MySystem.Instance.nowUserData.UnLockStore = true;
                SceneObjMgr.Instance.UpdateState();
                break;
            case 5:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.DryShooter);
                WhenGetPlantPannel.Instance.Show(PlantsType.DryShooter);
                break;
            case 6:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.SandShooter);
                WhenGetPlantPannel.Instance.Show(PlantsType.SandShooter);
                break;
            case 7:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.Aloes);
                WhenGetPlantPannel.Instance.Show(PlantsType.Aloes);
                break;
            case 8:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.GasterBlaster);
                WhenGetPlantPannel.Instance.Show(PlantsType.GasterBlaster);
                break;
            case 9:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.FireGrass);
                WhenGetPlantPannel.Instance.Show(PlantsType.FireGrass);
                break;
            case 10:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.DryFlower);
                WhenGetPlantPannel.Instance.Show(PlantsType.DryFlower);
                break;
            case 11:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.PeaPitcher);
                WhenGetPlantPannel.Instance.Show(PlantsType.PeaPitcher);
                break;
            case 12:

                MySystem.Instance.nowUserData.CardSlotCount += 1;
                break;
            case 13:
                MySystem.Instance.nowUserData.UnLockChest1 = true;
                SceneObjMgr.Instance.UpdateState();
                break;
            case 14:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.BloodChomper);
                WhenGetPlantPannel.Instance.Show(PlantsType.BloodChomper);
                break;
            case 15:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.SweetBerry);
                WhenGetPlantPannel.Instance.Show(PlantsType.SweetBerry);
                break;
            case 16:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.Dream);
                WhenGetPlantPannel.Instance.Show(PlantsType.Dream);
                break;
            case 17:
                UnlockPlants(PlantsType.LotusLeaf);
                break;
            case 18:
                UnlockPlants(PlantsType.Octopus);
                break;
            case 19:
                UnlockPlants(PlantsType.Crimson_Flower);
                break;
            case 20:
                UnlockPlants(PlantsType.PurifyCherry);
                break;
            case 21:
                UnlockPlants(PlantsType.Soul_Breaker);
                break;
            case 22:
                UnlockPlants(PlantsType.DeadFlower);
                break;
            case 23:
                UnlockPlants(PlantsType.ChargingShooter);
                break;
            case 24:
                UnlockPlants(PlantsType.Blood_MushRoom);
                break;
            case 25:
                MySystem.Instance.nowUserData.CardSlotCount += 1;
                break;
            case 200:
                MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.MachineGunShooter);
                WhenGetPlantPannel.Instance.Show(PlantsType.MachineGunShooter);
                break;

        }


        targetNode.UnLocked = true;
        slotMgr.ItemUpdate();
        Save();
        UpdateBNodeState();
    }
    private void UnlockPlants(PlantsType type)
    {
        MySystem.Instance.nowUserData.AddPlantInUserData(type);
        WhenGetPlantPannel.Instance.Show(type);
    }
    public void OnQuit()
    {
        SelectWindow.Instance.Cancel();
    }

    public void Load()//读取存档信息，然后赋值BNode节点
    {
        BNODE[] bnodes = new BNODE[transform.childCount];
        for(int i =0;i<bnodes.Length;i++)
        {
            bnodes[i] = transform.GetChild(i).gameObject.GetComponent<BNODE>();
        }

        for(int i = 0;i<MySystem.Instance.nowUserData.UnLockedNodes.Count;i++)
        {
            for(int j=0;j<bnodes.Length;j++)
            {
                if (bnodes[j].NodeID == MySystem.Instance.nowUserData.UnLockedNodes[i])
                {
                    bnodes[j].UnLocked = true;
                }
            }
        }
        if (bnodes[4].UnLocked == true)
        {
            MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.DryShooter);
        }
        if (bnodes[5].UnLocked == true)
        {
            MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.SandShooter);
        }
        if (bnodes[6].UnLocked == true)
        {
            MySystem.Instance.nowUserData.AddPlantInUserData(PlantsType.Aloes);
        }
        MySystem.Instance.SaveNowUserData();
    }
    public void Save()//获取BNode节点的状态，储存。
    {
        for(int i =0;i<transform.childCount;i++)
        {
            BNODE node = transform.GetChild(i).GetComponent<BNODE>();
            if(node.UnLocked == true)
            {
                MySystem.Instance.nowUserData.AddLockedNodes(node.NodeID);

            }
        }
        MySystem.Instance.SaveNowUserData();

    }
    public void UpdateBNodeState()//根据unLock的
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            BNODE tar = transform.GetChild(i).gameObject.GetComponent<BNODE>();
            if(i == 0)
            {
                tar.stage = 1;
            }

            if(tar.UnLocked == true)
            {
                for (int j = 0; j < tar.next.Length; j++)
                {
                    tar.next[j].stage = 1;
                }
                tar.stage = 2;
            }
            tar.ChangeStage();
            tar.ChangeLineColor();
        }

    }
    public void OpenNodeWindow()
    {
        NodeWindowAnimator.SetBool("enter", true);
    }
    public void UnLockButton()//解锁按钮
    {
        ButtonGizmos.sprite = lockSprites[0];
        ButtonText.text = "解锁";
        ButtonImage.sprite = BNodeButtonSprites[1];
    }
    public void LockButton()//锁住按钮
    {
        ButtonGizmos.sprite = lockSprites[1];
        ButtonText.text = "已解锁";
        ButtonImage.sprite = BNodeButtonSprites[2];
    }

}
