using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class R_Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public PlantsType type;
    [HideInInspector]public Sprite CardUnSelected;
    [HideInInspector] public Sprite CardSelected;
    [HideInInspector] private Image Cardimage;
    [HideInInspector] public Image plantImage;
    [HideInInspector] public Text CardConsumeText;
    private Plants plant;
    private Animator _animator;
    private Card TargetCard;
    [HideInInspector] public AudioClip[] tap;
    public bool IsSelected { get; private set; }
    private void Start()
    {
        #region GerCompoenet
        _animator = GetComponent<Animator>();
        plant = ResourceSystem.Instance.GetPlants(type).prefab.GetComponent<Plants>();
        Cardimage = GetComponent<Image>();
        
        #endregion

        Cardimage.sprite = CardUnSelected;
        plantImage.sprite = ResourceSystem.Instance.GetPlants(type).sprite;
        if (CardConsumeText != null)
            CardConsumeText.text = ResourceSystem.Instance.GetPlants(type).Consume.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected == false)
            OnCancleSelected();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }
    private void Select()//按下鼠标按键进行选择
    {
        if (CardSlot.Instance.CardNotFull() == true || IsSelected == true )
        {
            IsSelected = !IsSelected;
            if (IsSelected == true)
            {
                CardSlot.Instance.AddAnewCardInSlot(this);
                SoundSystem.Instance.Play2Dsound(tap[0]);
            }
            else
            {
                CardSlot.Instance.DeleteCardInSlot(TargetCard);
                SoundSystem.Instance.Play2Dsound(tap[1]);
                OnCancleSelected();
            }
        }

    }
    public void CancleSelect()//取消选择时调用
    {
        IsSelected = false;
        OnCancleSelected();
    }
    public void OnSelect()//鼠标进入时调用一次
    {
        _animator.SetBool("Select", true);
        Cardimage.sprite = CardSelected;
    }
    public void OnCancleSelected()//鼠标离开时调用一次
    {
        _animator.SetBool("Select", false);
        Cardimage.sprite = CardUnSelected;
    }
    public void SetTargetCard(Card card)
    {
        TargetCard = card;
    }

}
