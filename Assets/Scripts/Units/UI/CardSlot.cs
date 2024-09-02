using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LevelDataSpace;
public class CardSlot : BaseManager<CardSlot>
{
    public Text SunContText;
    public int SunCount;
    private List<Card> allCards = new List<Card>();
    public Transform CardsParent;
    public GameObject CardPrefab;
    public int MaxCardAmount = 6;
    public RectTransform CardSlotMiddleRect;
    private float DefaultWidth;
    public float CardWidth;
    private void Start()
    {

        //当捡到箱子时隐藏（事件）
        EventMgr.Instance.AddEventListener("PickUpChest", () => GetComponent<Animator>().SetBool("Quit",true));
        //
        if (MySystem.IsInLevel() == true)
        {
            ReFreshSunContText();
            GetAllCards();
            EventMgr.Instance.AddEventListener("GameStart", EnAbleAllCardsInSlot);
        }
        else
        {
            gameObject.SetActive(false);
        }
        //读取最大储存数，并且修改卡槽的贴图大小
        MaxCardAmount = MySystem.Instance.nowUserData.CardSlotCount;
        DefaultWidth = CardSlotMiddleRect.sizeDelta.x;
        CardSlotMiddleRect.sizeDelta = new Vector2(DefaultWidth + (MaxCardAmount - 6) * CardWidth, CardSlotMiddleRect.sizeDelta.y);
    }
    #region AboutSunCount
    public void SubSunCount(int SubAmount)//减去阳光数
    {
        SunCount -= SubAmount;
        ReFreshSunContText();
    }
    public void AddSunCont(int AddAmount)
    {
        SunCount += AddAmount;
        ReFreshSunContText();
    }

    public void SetSunCount(int Amount)
    {
        SunCount = Amount;
        ReFreshSunContText();
    }
    private void ReFreshSunContText()//刷新阳光 Text
    {
        SunContText.text = SunCount.ToString();
    }
    #endregion
    private void GetAllCards()//获取现有卡槽中所有的Card类
    {
        allCards.Clear();
        for (int i = 0; i < CardsParent.childCount; i++)
        {
            allCards.Add(CardsParent.GetChild(i).gameObject.GetComponent<Card>());
        }
    }
    public void AllCardsSetConsumeMask()//所有Card重新检测一次 阳光是否足够
    {
        GetAllCards();
        for (int i = 0; i < allCards.Count; i++)
        {
            allCards[i].SetConsumeMask();
        }

    }
    public Card AddAnewCardInSlot(R_Card r_card)
    {
        if (CardNotFull() == true)
        {
            GameObject c = Instantiate(CardPrefab, CardsParent);
            Card card = c.GetComponent<Card>();
            card.type = r_card.type;
            card.UnAble();
            r_card.SetTargetCard(card);
            GetAllCards();
            return card;
        }
        return null;

    }//在卡槽中新增一个卡片，并锁住
    public Card AddAnewCardInSlot(CardsInfo info)
    {

        if (CardNotFull() == true)
        {
            GameObject c = Instantiate(CardPrefab, CardsParent);
            Card card = c.GetComponent<Card>();
            card.type = info.plantType;
            card.nowCD = info.nowCD;
            card.CdOff = info.CdOff;
            card.GetOnce = info.GetOnce;
            card.EnAble();
            GetAllCards();
            return card;
        }
        return null;



    }//在卡槽中新增一个卡片，并且是解锁的状态
    public void DeleteCardInSlot(Card card)
    {
        Destroy(card.gameObject);
        GetAllCards();
    }//在卡槽里删除某一张卡片

    public void RemoveAllCardsInSlot()
    {
        GetAllCards();
        for (int i = 0; i < allCards.Count; i++)
        {
            DeleteCardInSlot(allCards[i]);
        }

    }
    public bool CardNotFull()
    {
        GetAllCards();
        if (allCards.Count < MaxCardAmount)
            return true;
        else
            return false;
    }//卡槽中的卡片未满时返回TRUE

    private void EnAbleAllCardsInSlot()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            allCards[i].EnAble();
        }
    }


}
