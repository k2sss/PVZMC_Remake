using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChestSlot : MonoBehaviour
{
    public Slot[] slots;
    public Scriptable_Levelinfo[] levelinfo;

    private ItemInfo[] items;
    private void Start()
    {
        slots = new Slot[transform.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = transform.GetChild(i).gameObject.GetComponent<Slot>();
        }
        for (int i = 0; i < levelinfo.Length; i++)
        {
            items = levelinfo[i].GetItem(MySystem.Instance.nowUserData.LevelType, MySystem.Instance.nowUserData.levelName);
            if(items != null&&items.Length > 0)
            for (int j = 0; j < items.Length; j++)
            {
                AddItems(items[j]);
            }

        }

    }
    public int AddItems(ItemInfo info)//�ڱ�������Ӷ����Ʒ
    {

        int counter = info.Count;
        bool TakeThing = false;
        for (int i = 0; i < info.Count; i++)
        {
            if (AddANewItem(info))//�����
            {
                counter -= 1;
                TakeThing = true;
            }
            else
            {
                break;
            }
        }
        if (TakeThing == true)
        {
            //SoundSystem.Instance.Play2Dsound(pop);
        }
        EventMgr.Instance.EventTrigger("OnChangeInventory");
        return counter;//����ʣ�಻��װ�µ���
    }
    private bool AddANewItem(ItemInfo info)
    {
        int aaaa = 0;
        if (!CheckHasItem(info.type, ref aaaa))//�Ҳ�����Ӧ��
        {
            int slotID = 0;
            if (FindEmptySlot(ref slotID))
            {
                slots[slotID].AddNewItem(info);
                return true;
            }
        }
        else//�ҵ���Ӧ��
        {
            int slotID = 0;
            CheckHasItem(info.type, ref slotID);
            slots[slotID].AddItem();
            return true;
        }
        return false;


    }//�ڱ�������ӵ�����Ʒ
    public bool CheckHasItem(ItemType type, ref int num)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].CheckCanAddItem(type))
            {
                num = i;
                return true;
            }
        }
        return false;

    }//��鱳�����Ƿ��ж�Ӧ��Ʒ(��������)����ͨ�����ô�����������Ӷ�Ӧ��ID
    public bool FindEmptySlot(ref int num)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                num = i;
                return true;
            }
        }
        return false;
    }//��鱳���Ƿ��п�λ�������ص�һ����λ��ID
}
