using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Image HighLightImage;
    public float HighLightAlpha = 0.2f;
    protected Image ItemImage;
    protected Text CountText;
    protected Image DurabilityBar;
    protected Image DurabilityBotton;
    public ItemInfo info;
    protected bool IsEnter;
    protected bool HasInputMouseButton;
    private float Animtimer;
    private int AnimFrame;
    public Action onClick;
    public Action onItemUpdate;
    protected ItemDrag MyItemDrag;
    protected ItemInspector MyItemInspector;
    protected virtual void Awake()
    {
        HighLightImage = transform.GetChild(1).GetComponent<Image>();
        ItemImage = transform.GetChild(0).GetComponent<Image>();
        CountText = transform.GetChild(2).GetComponent<Text>();
        DurabilityBotton = transform.GetChild(3).GetComponent<Image>();
        DurabilityBar = transform.GetChild(3).GetChild(0).GetComponent<Image>();
    }
    public virtual void Start()
    {
        ItemUpdate();
        MyItemDrag = ItemDrag.Instance;
        MyItemInspector = ItemInspector.Instance;
    }

    public virtual void Update()
    {
        if (IsEnter == true)
        {
            HasInputMouseButton = false;
            if (HasInputMouseButton == false)
                ExChangeItem();
            if (HasInputMouseButton == false)
                AddItemFromItemDrag();
            if (HasInputMouseButton == false)
                GetOneItemFromItemDrag();
            if (HasInputMouseButton == false)
                PickUpHalfItem();
            if (HasInputMouseButton == true)
            {
                EventMgr.Instance.EventTrigger("OnChangeInventory");
            }
        }
        AnimationSprite();
    }
    public void AnimationSprite()
    {
        if (ResourceSystem.Instance.GetItem(info).AnimationSprites.Count > 1)
        {
            Animtimer += Time.unscaledDeltaTime;
            if (Animtimer >= ResourceSystem.Instance.GetItem(info).AnimationGapTime)
            {
                Animtimer = 0;
                ChangeSprite(ResourceSystem.Instance.GetItem(info).AnimationSprites[AnimFrame]);
                AnimFrame++;
                if (AnimFrame >= ResourceSystem.Instance.GetItem(info).AnimationSprites.Count)
                {
                    AnimFrame = 0;
                }
            }
        }


    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        HighLightImage.color = new Vector4(1, 1, 1, HighLightAlpha);
        IsEnter = true;
        MyItemInspector.CheckItem(info.type);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        CancleEnter();

    }
    public void CancleEnter()
    {
        HighLightImage.color = new Vector4(1, 1, 1, 0);
        IsEnter = false;
        MyItemInspector.CheckItem(ItemType.Nothing);
    }
    public virtual void GetOneItemFromItemDrag()//��ItemDrag��Ϊ��ʱ���ұ�SlotΪ�ջ���Ϊ��ͬ���ͣ��������Ҽ���һ����Ʒ
    {
        if (Input.GetMouseButtonDown(1) && !MyItemDrag.IsEmpty() && (IsEmpty() || MyItemDrag.CompareType(info)))
        {
            if (IsEmpty())
            {
                AddNewItem(MyItemDrag.info.ShallowClone());
                MyItemDrag.SubCount(1);
            }
            else if (MyItemDrag.CompareType(info))
            {
                AddItem();
                MyItemDrag.SubCount(1);
            }
            HasInputMouseButton = true;
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            onClick?.Invoke();
        }
    }
    public virtual void PickUpHalfItem()//��ItemDragΪ��ʱ���������Ҽ���һ����Ʒת�Ƶ�ItemDrag
    {

        if (Input.GetMouseButtonDown(1) && MyItemDrag.IsEmpty())
        {
            ItemInfo t = info.ShallowClone();
            t.Count /= 2;
            info.Count = info.Count - t.Count;
            MyItemDrag.info = info.ShallowClone();
            info = t;
            //Debug.Log(info.type);
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            HasInputMouseButton = true;
            onClick?.Invoke();
        }
    }
    public virtual void ExChangeItem()//��ItemDrag��Type�ͱ�slot��һ��ʱ,��ItemDrag�е�Info�ͱ�Slot��Info����
    {
        if (Input.GetMouseButtonDown(0) && !MyItemDrag.CompareType(info))
        {
            ItemInfo t = info.ShallowClone();
            info = MyItemDrag.info.ShallowClone();
            MyItemDrag.info = t;
            HasInputMouseButton = true;
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            onClick?.Invoke();
        }
    }
    public virtual void AddItemFromItemDrag()//��ItemDrag��Type�ͱ�slotһ��ʱ,������������е��Ӳ���
    {
        if (Input.GetMouseButtonDown(0) && MyItemDrag.CompareType(info))
        {
            if (info.Count + MyItemDrag.info.Count > ResourceSystem.Instance.GetItem(info).MaxAmount)//�����
            {
                if (ResourceSystem.Instance.GetItem(info).MaxAmount != 1)
                {
                    MyItemDrag.info.Count = MyItemDrag.info.Count - (ResourceSystem.Instance.GetItem(info).MaxAmount - info.Count);
                    info.Count = ResourceSystem.Instance.GetItem(info).MaxAmount;
                }
            }
            else
            {
                info.Count += MyItemDrag.info.Count;
                MyItemDrag.CleanItem();
            }
            HasInputMouseButton = true;
            ItemUpdate();
            MyItemDrag.ItemUpdate();
            onClick?.Invoke();
        }
    }

    public void CleanItem()//�����Ʒ
    {
        info.Count = 0;
        info.type = ItemType.Nothing;
        info.Durability = 0;
        ItemUpdate();
        EventMgr.Instance.EventTrigger("OnChangeInventory");
    }
    public void AddNewItem(ItemInfo info)//���һ������Ʒ
    {

        this.info.type = info.type;
        this.info.Count += 1;
        this.info.Durability = info.Durability;
        ChangeSprite(ResourceSystem.Instance.GetItem(info).AnimationSprites[0]);
        ItemUpdate();
    }
    public void AddItem()//���һ�����е���Ʒ
    {
        this.info.Count += 1;
        ItemUpdate();
    }
    public bool CheckCanAddItem(ItemType type)
    {
        if (this.info.type == type && this.info.Count < ResourceSystem.Instance.GetItem(info).MaxAmount)
        {
            return true;
        }
        return false;
    }//����Ƿ�������Ʒ�����Slot(��Ϊ��)
    public int CheckSlotVolume(ItemType type)//�������
    {
        if (info.type == type)
        {
            return ResourceSystem.Instance.GetItem(info).MaxAmount - info.Count;
        }
        else if (info.type == ItemType.Nothing)
        {
            return ResourceSystem.Instance.GetItem(info).MaxAmount;
        }
        return 0;

    }//����Ƿ�������Ʒ�����Slot(��Ϊ��)
    public void ChangeSprite(Sprite sprite)
    {
        ItemImage.sprite = sprite;
        ItemImage.color = Color.white;
    }//������Ʒ��ͼ
    public bool IsEmpty()
    {
        if (this.info.type == ItemType.Nothing)
        {
            return true;
        }
        return false;
    }//������Slot�Ƿ�Ϊ��

    public void ItemUpdate()
    {
        if (info.Count <= 0 || info.Durability <= 0)
        {
            info.Durability = 0;
            info.Count = 0;
            info.type = ItemType.Nothing;
        }
        ReFreshCountText();
        ReFreshDurabilityBar();
        ReFreshItemSprite();
        onItemUpdate?.Invoke();

    }//����Item״̬
    public virtual void ReFreshCountText()
    {
        if (info.Count <= 1)
        {
            CountText.text = "";
        }
        else
        {
            CountText.text = info.Count.ToString();
        }
    }//����Item������ʾ
    public void ReFreshDurabilityBar()
    {

        if (info.type == ItemType.Nothing || info.Durability == 0 || info.Durability == ResourceSystem.Instance.GetItem(info).MaxDurability)
        {
            DurabilityBotton.color = new Color(0, 0, 0, 0);
            DurabilityBar.fillAmount = 0;
        }
        else
        {
            DurabilityBotton.color = new Color(0, 0, 0, 1);
            DurabilityBar.fillAmount = (float)info.Durability / ResourceSystem.Instance.GetItem(info).MaxDurability;
            float f = DurabilityBar.fillAmount;
            if (f <= 1f && f > 0.5f)
            {
                DurabilityBar.color = Color.green;
            }
            else if (f <= 0.5f && f > 0.2f)
            {
                DurabilityBar.color = Color.yellow;
            }
            else
            {
                DurabilityBar.color = Color.red;
            }
        }


    }//����Item�;�����ʾ

    public void ReFreshItemSprite()
    {
        AnimFrame = 0;
        ChangeSprite(ResourceSystem.Instance.GetItem(info.type).AnimationSprites[0]);
    }

    public virtual void OnDisable()
    {
        CancleEnter();
    }
}
