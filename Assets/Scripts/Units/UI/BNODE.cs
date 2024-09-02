using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BNODE : MonoBehaviour
{
    //public string nodeName;
    
    public bool UnLocked;
    public int NodeID;
    public BNODE[] next;
    public Image targetLineImg;
    private Image buttonImage;
    public int stage = 0;
    public NodeMgr nodeMgr;
    //public string nodeName;
    [Multiline]
    public string nodeDescription;
    public ItemInfo[] neededItems;
    public void ChangeStage()
    {
        buttonImage = transform.Find("NodeButton").gameObject.GetComponent<Image>();
        buttonImage.sprite = nodeMgr.BNodeButtonSprites[stage];
       
    }
    public void ChangeLineColor()
    {
        if(targetLineImg != null)
        {
            switch(stage)
            {
                case 0:
                    targetLineImg.color = Color.black;
                    break;
                case 1:
                    targetLineImg.color = Color.white;
                    break;
                case 2:
                    targetLineImg.color = new Color(0,1,0.8f);
                    break;
            }

         }
    }
    public void Clicked()
    {
        if (stage == 1|| stage == 2)
        {
            nodeMgr.OpenNodeWindow();
            nodeMgr.targetNode = this;
            nodeMgr.nodeNameText.text = transform.Find("NodeButton").transform.Find("Text (Legacy)").GetComponent<Text>().text;
            nodeMgr.nodeDescriptionTextPrinter.Sentences[0] = nodeDescription;
            nodeMgr.nodeDescriptionTextPrinter.TextIndexReset();
            nodeMgr.nodeDescriptionTextPrinter.StartPrintNextSentence();
            if(stage == 1)
            {
                nodeMgr.UnLockButton();
                
            }
            else
            if (stage == 2)
            {
                nodeMgr.LockButton();
                ItemInfo[] itemInfo = new ItemInfo[0];
                nodeMgr.slotMgr.Give(itemInfo);
            }
            nodeMgr.slotMgr.Give(neededItems);
        }

    }
}
