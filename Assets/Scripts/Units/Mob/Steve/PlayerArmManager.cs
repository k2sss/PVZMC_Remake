using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerArmor
{
    public class PlayerArmManager : MonoBehaviour
    {
        private ArmorSlot[] armorSlots;
        [HideInInspector] public ArmorDatas armorDatas;
        private PlayerHealth player;
        private string ResourcePath = "SciptableObjects/PlayerArmorData";
        private List<GameObject> armorList = new List<GameObject>();
        private List<BuffType> armorbufftypes = new List<BuffType>();
        [HideInInspector] public Transform headParent;
        [HideInInspector] public Transform chestParent;
        [HideInInspector] public Transform LeftLegParent;
        [HideInInspector] public Transform RightLegParent;
        [HideInInspector] public Transform LeftShoeParent;
        [HideInInspector] public Transform RightShoeParent;
        [HideInInspector] public Transform LeftArmParent;
        [HideInInspector] public Transform RightArmParent;

        private void Awake()
        {
            GetResources();
            GetParents();
        }
        private void Start()
        {
            GetSlot();
            if (PlayerHealth.instance != null)
                player = PlayerHealth.instance;
            EventMgr.Instance.AddEventListener("UpdateArmor", UpdateInfo);

            Load();

            if (!MySystem.IsInLevel())
            {

                MySystem.Instance.whenSaveAction += Save;
            }
        }
        private void Load()
        {
            //护甲格
            if (InventoryManager.Instance != null && MySystem.Instance.nowUserData.armorItems.Length > 0)
            {
                Transform parent = InventoryManager.Instance.transform.Find("Inventory/Inventory_Main/ArmorSlots");
                for (int i = 0; i < parent.childCount; i++)
                {
                    parent.GetChild(i).gameObject.GetComponent<ArmorSlot>().info = MySystem.Instance.nowUserData.armorItems[i].ShallowClone();
                }
            }

            UpdateInfo();
        }
        public void Save()
        {
            if (InventoryManager.Instance != null)
            {

                Transform slotParent = InventoryManager.Instance.transform.Find("Inventory/Inventory_Main/ArmorSlots");
                MySystem.Instance.nowUserData.armorItems = new ItemInfo[slotParent.childCount];
                for (int i = 0; i < slotParent.childCount; i++)
                {
                    ArmorSlot slot = slotParent.GetChild(i).gameObject.GetComponent<ArmorSlot>();
                    MySystem.Instance.nowUserData.armorItems[i] = new ItemInfo(slot.info.type, slot.info.Count, slot.info.Durability);
                }
            }
        }
        private void GetSlot()
        {
            if (InventoryManager.Instance != null)
            {
                Transform parent = InventoryManager.Instance.transform.Find("Inventory/Inventory_Main/ArmorSlots");
                armorSlots = new ArmorSlot[parent.transform.childCount];
                for (int i = 0; i < armorSlots.Length; i++)
                {
                    armorSlots[i] = parent.transform.GetChild(i).gameObject.GetComponent<ArmorSlot>();
                }
            }

        }
        private void GetResources()
        {
            armorDatas = FileLoadSystem.ResourcesLoad<ArmorDatas>(ResourcePath);
        }
        private void GetParents()
        {
            headParent = transform.Find("SimplePlayer.arma/MAIN/center/Body/Chest/Head").transform;
            chestParent = transform.Find("SimplePlayer.arma/MAIN/center/Body/Chest").transform;
            LeftLegParent = transform.Find("SimplePlayer.arma/MAIN/center/Leg:Left:Upper").transform;
            RightLegParent = transform.Find("SimplePlayer.arma/MAIN/center/Leg:Right:Upper").transform;
            LeftShoeParent = transform.Find("SimplePlayer.arma/MAIN/center/Leg:Left:Upper/Leg:Left:Lower/Leg:Left:Lower_end").transform;
            RightShoeParent = transform.Find("SimplePlayer.arma/MAIN/center/Leg:Right:Upper/Leg:Right:Lower/Leg:Right:Lower_end").transform;
            LeftArmParent = transform.Find("SimplePlayer.arma/MAIN/center/Body/Chest/Arm:Left:Upper").transform;
            RightArmParent = transform.Find("SimplePlayer.arma/MAIN/center/Body/Chest/Arm:Right:Upper").transform;
        }
        /// <summary>
        /// 根据枚举类型获取父节点
        /// </summary>
        /// <returns></returns>
        private Transform GetTransform(ArmorParentType type)
        {
            switch (type)
            {
                case ArmorParentType.head:
                    return headParent;
                case ArmorParentType.chest:
                    return chestParent;
                case ArmorParentType.leftLeg:
                    return LeftLegParent;
                case ArmorParentType.rightLeg:
                    return RightLegParent;
                case ArmorParentType.leftShoe:
                    return LeftShoeParent;
                case ArmorParentType.rightShoe:
                    return RightShoeParent;
                case ArmorParentType.leftArm:
                    return LeftArmParent;
                case ArmorParentType.rightArm:
                    return RightArmParent;
            }
            return null;
        }
        /// <summary>
        /// 更新一次 护甲状态
        /// </summary>
        private void UpdateInfo()
        {
            int totalDefence = 0;
            if (gameObject.CompareTag("Player"))
            {
                //清除护甲增益
                for (int i = 0; i < armorbufftypes.Count; i++)
                {
                    player.StopBuff(armorbufftypes[i]);
                }
                //清除护甲
                
                
            }
            for (int i = 0; i < armorList.Count; i++)
                {
                    Destroy(armorList[i]);
                }
                armorList.Clear();
            int[] armorParents = new int[armorSlots.Length];
            //检查单个护甲
            
            for (int i = 0; i < armorSlots.Length; i++)
            {
                if (armorSlots[i].info.type != ItemType.Nothing)
                {
                    ArmorGroup group = armorDatas.FindTheAromorGroup(armorSlots[i].info.type, ref armorParents[i]);
                    totalDefence += group.ArmorDefence;
                    for (int j = 0; j < group.armors.Length; j++)
                    {
                        //生成护甲
                        GameObject a = Instantiate(group.armors[j].ArmorModel, GetTransform(group.armors[j].type));
                        a.layer = 13;
                        armorList.Add(a);
                    }
                    for (int m = 0; m < group.bufftype.Length; m++)
                    {
                        player.AddBuff(group.bufftype[m]);
                        armorbufftypes.Add(group.bufftype[m]);
                    }

                }
            }
            if (gameObject.CompareTag("Player"))
            {
                //检查护甲是否为一个套装
                int flag = armorParents[0];
                int flag2 = 0;
                for (int i = 0; i < armorParents.Length; i++)
                {
                    if (flag == armorParents[i])
                    {
                        flag2++;
                    }
                }
                if (flag2 == armorParents.Length)
                {
                    for (int m = 0; m < armorDatas.datas[flag].buffType.Length; m++)
                    {
                        //Debug.Log("已获取套装增益" + armorDatas.datas[flag].buffType[m]);
                        player.AddBuff(armorDatas.datas[flag].buffType[m]);
                        armorbufftypes.Add(armorDatas.datas[flag].buffType[m]);
                    }
                }

                player.Defence = totalDefence;

                if (ArmorGizmos.instance != null)
                {
                    ArmorGizmos.instance.Init();
                    ArmorGizmos.instance.AllInfoUpdate();
                }
            }

        }
    }
    public enum ArmorParentType
    {
        head,
        chest,
        leftLeg,
        rightLeg,
        leftShoe,
        rightShoe,
        leftArm,
        rightArm,
    }

    [System.Serializable]
    public class PlayerArmorData
    {
        public string ArmorGroupName;
        public ArmorGroup[] groups;
        public BuffType[] buffType;//套装Buff
    }
    [System.Serializable]
    public class ArmorGroup
    {

        public string ArmorGroupName;
        public ArmorType armorType;
        public ItemType targetItemType;//对应的物品
        public BuffType[] bufftype;//单个护甲的BUFF加成
        public int ArmorDefence;//护甲值
        public Armor[] armors;
    }

    [System.Serializable]
    public class Armor
    {
        public ArmorParentType type;
        public GameObject ArmorModel;
    }
}
