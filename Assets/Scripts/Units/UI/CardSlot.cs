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

        //��������ʱ���أ��¼���
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
        //��ȡ��󴢴����������޸Ŀ��۵���ͼ��С
        MaxCardAmount = MySystem.Instance.nowUserData.CardSlotCount;
        DefaultWidth = CardSlotMiddleRect.sizeDelta.x;
        CardSlotMiddleRect.sizeDelta = new Vector2(DefaultWidth + (MaxCardAmount - 6) * CardWidth, CardSlotMiddleRect.sizeDelta.y);
    }
    #region AboutSunCount
    public void SubSunCount(int SubAmount)//��ȥ������
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
    private void ReFreshSunContText()//ˢ������ Text
    {
        SunContText.text = SunCount.ToString();
    }
    #endregion
    private void GetAllCards()//��ȡ���п��������е�Card��
    {
        allCards.Clear();
        for (int i = 0; i < CardsParent.childCount; i++)
        {
            allCards.Add(CardsParent.GetChild(i).gameObject.GetComponent<Card>());
        }
    }
    public void AllCardsSetConsumeMask()//����Card���¼��һ�� �����Ƿ��㹻
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

    }//�ڿ���������һ����Ƭ������ס
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



    }//�ڿ���������һ����Ƭ�������ǽ�����״̬
    public void DeleteCardInSlot(Card card)
    {
        Destroy(card.gameObject);
        GetAllCards();
    }//�ڿ�����ɾ��ĳһ�ſ�Ƭ

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
    }//�����еĿ�Ƭδ��ʱ����TRUE

    private void EnAbleAllCardsInSlot()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            allCards[i].EnAble();
        }
    }


}
