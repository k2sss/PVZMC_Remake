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
    public virtual void GetOneItemFromItemDrag()//当ItemDrag不为空时，且本Slot为空或者为相同类型，点击鼠标右键加一个物品
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
    public virtual void PickUpHalfItem()//当ItemDrag为空时，点击鼠标右键将一半物品转移到ItemDrag
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
    public virtual void ExChangeItem()//当ItemDrag的Type和本slot不一样时,将ItemDrag中的Info和本Slot的Info交换
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
    public virtual void AddItemFromItemDrag()//当ItemDrag的Type和本slot一样时,点击鼠标左键进行叠加补充
    {
        if (Input.GetMouseButtonDown(0) && MyItemDrag.CompareType(info))
        {
            if (info.Count + MyItemDrag.info.Count > ResourceSystem.Instance.GetItem(info).MaxAmount)//有溢出
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

    public void CleanItem()//清除物品
    {
        info.Count = 0;
        info.type = ItemType.Nothing;
        info.Durability = 0;
        ItemUpdate();
        EventMgr.Instance.EventTrigger("OnChangeInventory");
    }
    public void AddNewItem(ItemInfo info)//添加一个新物品
    {

        this.info.type = info.type;
        this.info.Count += 1;
        this.info.Durability = info.Durability;
        ChangeSprite(ResourceSystem.Instance.GetItem(info).AnimationSprites[0]);
        ItemUpdate();
    }
    public void AddItem()//添加一个已有的物品
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
    }//检查是否可添加物品到这个Slot(不为空)
    public int CheckSlotVolume(ItemType type)//检查容量
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

    }//检查是否可添加物品到这个Slot(不为空)
    public void ChangeSprite(Sprite sprite)
    {
        ItemImage.sprite = sprite;
        ItemImage.color = Color.white;
    }//更改物品贴图
    public bool IsEmpty()
    {
        if (this.info.type == ItemType.Nothing)
        {
            return true;
        }
        return false;
    }//检查这个Slot是否为空

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

    }//更新Item状态
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
    }//更新Item数量显示
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


    }//更新Item耐久条显示

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
