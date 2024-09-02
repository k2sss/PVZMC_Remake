using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public PlantsType type;//0
    public Sprite CardUnSelected;
    public Sprite CardSelected;
    private Image Cardimage;
    public Image plantImage;
    public Image CDMask;
    public Image ConsumeMask;
    public Text CardConsumeText;
    private Plants plant;
    public float nowCD;//0
    public float MaxCD { get; private set; }
    public bool CdOff;//0
    private Animator _animator;
    private bool IsAble;
    [HideInInspector] public bool GetOnce;
    public AudioClip[] tap;
    public AudioClip[] plantSound;

    public bool IsSelected { get; private set; }
    private void Start()
    {
        #region GerCompoenet
        plant = ResourceSystem.Instance.GetPlants(type).prefab.GetComponent<Plants>();
        Cardimage = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        #endregion

        Cardimage.sprite = CardUnSelected;
        plantImage.sprite = ResourceSystem.Instance.GetPlants(type).sprite;
        if (CardConsumeText != null)
            CardConsumeText.text = ResourceSystem.Instance.GetPlants(plant.type).Consume.ToString();

        MaxCD = ResourceSystem.Instance.GetPlants(plant.type).CD;
        if (ResourceSystem.Instance.GetPlants(plant.type).StartWithNoCD == true&&GetOnce == false)
        {
            nowCD = MaxCD;
        }

    }
    private void Update()
    {
        if(IsAble == true)
        SetCDMask();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (IsAble == true)
        {
            if (PlantManager.Instance.IsSelectedPlant() == false && CheckCanPlant() == true)
                OnSelect();
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsAble == true)
        {
            if (IsSelected == false)
                OnCancleSelected();
        }

    }

    public void OnPointerDown(PointerEventData eventData)//
    {
        if (IsAble == true)
        {
            if (PlantManager.Instance.IsSelectedPlant() == false && CheckCanPlant() == true)
                OnSelect();

            if ((PlantManager.Instance.IsSelectedPlant() == false || IsSelected == true) && CheckCanPlant() == true)
                Select();
        }
    }
    private void Select()//按下鼠标按键进行选择
    {

        IsSelected = !IsSelected;
        if (IsSelected == true)
        {
            PlantManager.Instance.GetSelectedPlant(type, this);
            SoundSystem.Instance.Play2Dsound(tap[0]);
        }
        else
        {
            PlantManager.Instance.EmptySelectedPlant();
            SoundSystem.Instance.Play2Dsound(tap[1]);
        }

    }
    public void CancleSelect()//取消选择时调用
    {
        IsSelected = false;
        
        OnCancleSelected();
    }
    public virtual void OnPlant()//当植物种植时调用一次
    {
        nowCD = 0;
        CdOff = false;
        SoundSystem.Instance.PlayRandom2Dsound(plantSound);
    }

    protected virtual bool CheckCanPlant()//检查该卡片是否符合种植条件
    {
        if (CardSlot.Instance.SunCount >= ResourceSystem.Instance.GetPlants(type).Consume && nowCD > MaxCD)
            return true;
        return false;
    }
    private void SetCDMask()//设置CD黑色遮罩
    {
        if (nowCD <= MaxCD)
            nowCD += Time.deltaTime;
        else
        {
            OnCdOff();
        }

        CDMask.fillAmount = 1 - nowCD / MaxCD;
    }
    private void OnCdOff()//nowCd为MaxCd时调用一次
    {
        if (CdOff == false)
        {
            _animator.SetBool("CdOff", true);
            Invoke("CancleCdOffAnimation", 0.1f);
            CdOff = true;
        }

    }
    private void CancleCdOffAnimation()
    {
        _animator.SetBool("CdOff", false);
    }
    public void SetConsumeMask()//设置遮罩
    {
        if (CardSlot.Instance.SunCount >= ResourceSystem.Instance.GetPlants(type).Consume)
        {
            ConsumeMask.color = new Color(0, 0, 0, 0);
        }
        else
        {
            ConsumeMask.color = new Color(0, 0, 0, 0.5f);
        }
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
        if(CardSlot.Instance !=null)
        CardSlot.Instance.AllCardsSetConsumeMask();
    }

    public void UnAble()//禁用该卡片
    {
        IsAble = false;
    }
    public void EnAble()//启用这个卡片
    {
        IsAble = true;
    }

}
