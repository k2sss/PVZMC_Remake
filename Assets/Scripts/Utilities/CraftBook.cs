using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBook : MonoBehaviour
{
    public Transform ContentParent { get; private set; }
    public GameObject slot { get; private set; }
    public CraftTable craftTable;
    public List<CraftBookSlot> slots = new List<CraftBookSlot>();
    public int SelectCraftBookSlotID;
    private void Awake()
    {
        ContentParent = transform.GetChild(0).transform.GetChild(0).transform;
        slot = FileLoadSystem.ResourcesLoad<GameObject>("CraftSlot");
    }
    void Start()
    {
        EventMgr.Instance.AddEventListener("OnChangeInventory", CheckInventory);
        //Invoke("CheckInventory",);
        MonoController.Instance.InvokeUnScaled(0.01f, CheckInventory);
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void SetCraftTable(CraftTable table)
    {
        craftTable = table;
    }
    public void CheckInventory()//������Awake������ִ�� ==> 
    {
        //����ÿһ���ϳɱ�
        CleanAllSlot();
        InventoryManager.Instance.UpdateItemDic();
        for (int i = 0; i < CraftManager.Instance.CraftInfos.Count; i++)
        {
            //itemDic_Craft������ÿ���ϳɱ�����Ҫ�Ĳ����Լ�ÿ�ֲ��ϵĸ���

            Dictionary<ItemType, int> itemDic_Craft = new Dictionary<ItemType, int>();
            for (int j = 0; j < 9; j++)
            {
                if (CraftManager.Instance.CraftInfos[i].types[j] != ItemType.Nothing)
                {
                    if (!itemDic_Craft.ContainsKey(CraftManager.Instance.CraftInfos[i].types[j]))
                    {

                        itemDic_Craft.Add(CraftManager.Instance.CraftInfos[i].types[j], 1);
                    }
                    else
                    {
                        itemDic_Craft[CraftManager.Instance.CraftInfos[i].types[j]]++;
                    }
                }

            }
            //����itemDic_Craft

            bool create = false;
            foreach (ItemType type in itemDic_Craft.Keys)
            {
                if (InventoryManager.Instance.GetItemCount(type) > 0)
                {
                    create = true;
                    break;
                }
            }
            bool flag = true;
            foreach (ItemType type in itemDic_Craft.Keys)
            {
                //������� ���� �ṩ�����ж�Ϊ���ϲ���
                if (itemDic_Craft[type] > InventoryManager.Instance.GetItemCount(type))
                {
                    flag = false;
                    break;
                }

            }
            //�ж��Ƿ�����
            if (create == true)
                CreateASlot(CraftManager.Instance.CraftInfos[i], flag,i);

        }


    }
    private void CreateASlot(CraftInfo info, bool IsAble,int ID)//�����ഴ��һ��SLOT
    {
        GameObject obj = Instantiate(slot, ContentParent);
        CraftBookSlot s = obj.GetComponent<CraftBookSlot>();
        s.info.type = info.OutPutType;
        s.info.Durability = ResourceSystem.Instance.GetItem(info.OutPutType).MaxDurability;
        s.info.Count = 1;
        s.IsAble = IsAble;
        s.C_info = info;
        s.ID = ID;
        s.book = this;
        slots.Add(s);
    }
    private void CleanAllSlot()//���slot
    {
        slots.Clear();
        for (int i = 0; i < ContentParent.transform.childCount; i++)
        {
            Destroy(ContentParent.GetChild(i).gameObject);
        }
    }
 
}
