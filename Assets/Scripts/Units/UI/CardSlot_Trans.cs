using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot_Trans : MonoBehaviour
{
    public Transform CardsParent;
    public GameObject CardPrefab;
    public int MaxCardAmount = 6;
    public float CardWidth;
    public RectTransform leftSide;
    public RectTransform rightSide;
    public RectTransform CardSummonPlace;
    public float CardMoveSpeed = 3;
    public float timer;
    private float timeInterval = 5;
    private PlantsType[] types;
    private void Awake()
    {
       gameObject.SetActive(false);
    }
    public void Init(float timeInterval, PlantsType[] types)
    {
        this.timeInterval = timeInterval;
        this.types = types;
        gameObject.SetActive(true);
        EventMgr.Instance.AddEventListener("PickUpChest", () => GetComponent<Animator>().SetBool("Quit", true));
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeInterval)
        {
            timer = 0;
            CreateANewCard(types[Random.Range(0,types.Length)]);
        }
        MakeCardMove();
    }
    public bool IsFull()
    {
        if ((rightSide.anchoredPosition.x - leftSide.anchoredPosition.x) < CardsParent.transform.childCount * CardWidth)
        {
            return true;
        }
        return false;
    }
    private void MakeCardMove()
    {
        for (int i = 0; i < CardsParent.childCount; i++)
        {
            RectTransform cardTransform = CardsParent.GetChild(i).GetComponent<RectTransform>();
            if(cardTransform.anchoredPosition.x >(leftSide.anchoredPosition.x + CardWidth/2 + i*CardWidth))
            cardTransform.anchoredPosition -= new Vector2(CardMoveSpeed * Time.deltaTime, 0);
            
        }


    }
    public void CreateANewCard(PlantsType type)
    {
        if (IsFull() == false)
        {
            GameObject newCardObj = Instantiate(CardPrefab, CardsParent);
            Card card = newCardObj.GetComponent<Card>();
            card.type = type;
            card.EnAble();
            card.GetComponent<RectTransform>().position= CardSummonPlace.position;
        }
    }

}
