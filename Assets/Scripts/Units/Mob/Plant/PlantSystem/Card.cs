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
    private void Select()//������갴������ѡ��
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
    public void CancleSelect()//ȡ��ѡ��ʱ����
    {
        IsSelected = false;
        
        OnCancleSelected();
    }
    public virtual void OnPlant()//��ֲ����ֲʱ����һ��
    {
        nowCD = 0;
        CdOff = false;
        SoundSystem.Instance.PlayRandom2Dsound(plantSound);
    }

    protected virtual bool CheckCanPlant()//���ÿ�Ƭ�Ƿ������ֲ����
    {
        if (CardSlot.Instance.SunCount >= ResourceSystem.Instance.GetPlants(type).Consume && nowCD > MaxCD)
            return true;
        return false;
    }
    private void SetCDMask()//����CD��ɫ����
    {
        if (nowCD <= MaxCD)
            nowCD += Time.deltaTime;
        else
        {
            OnCdOff();
        }

        CDMask.fillAmount = 1 - nowCD / MaxCD;
    }
    private void OnCdOff()//nowCdΪMaxCdʱ����һ��
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
    public void SetConsumeMask()//��������
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
    public void OnSelect()//������ʱ����һ��
    {
        _animator.SetBool("Select", true);
        Cardimage.sprite = CardSelected;
    }
    public void OnCancleSelected()//����뿪ʱ����һ��
    {
        _animator.SetBool("Select", false);
        Cardimage.sprite = CardUnSelected;
        if(CardSlot.Instance !=null)
        CardSlot.Instance.AllCardsSetConsumeMask();
    }

    public void UnAble()//���øÿ�Ƭ
    {
        IsAble = false;
    }
    public void EnAble()//���������Ƭ
    {
        IsAble = true;
    }

}
