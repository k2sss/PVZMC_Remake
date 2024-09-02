using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class ItemCreateTookit : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private IntegerField itemIdField;
    
    private GameObject itemGameObjectOrigin;
    private GameObject itemGameObjectOrigin_inHand;
    private Button createButton;
    private Button deleteButton;
    private TextField itemNameField;
    private ObjectField objectField_Sprite;
    private IntegerField itemDurabilityFiled;
    private IntegerField itemAmountFiled;
    private ColorField colorField;
    [MenuItem("K2S/ItemCreateTookit")]
    public static void ShowExample()
    {
        ItemCreateTookit wnd = GetWindow<ItemCreateTookit>();
        wnd.titleContent = new GUIContent("ItemCreateTookit");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
       
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ItemCreateTookit.uxml");
        visualTree.CloneTree(root);
        

        itemGameObjectOrigin = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Item/Origin.prefab");
        itemGameObjectOrigin_inHand = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Item/OriginInhand.prefab");
       // ItemtypeField = root.Q<EnumField>("ItemType");
        itemIdField = root.Q<IntegerField>("ItemID");
        itemNameField = root.Q<TextField>("ItemName");
        createButton = root.Q<Button>("Create");
        createButton.clicked += CreateItem;
        deleteButton = root.Q<Button>("Delete");
        deleteButton.clicked += DeleteItem;
        objectField_Sprite = root.Q<ObjectField>("tarSprite");
        colorField = root.Q<ColorField>("textColor");
        itemDurabilityFiled = root.Q<IntegerField>("durability");
        itemAmountFiled = root.Q<IntegerField>("maxAmount");
      


    }
    private string savePath = "Assets/Prefabs/Item/"; // 预制体保存路径

    private void SaveObjectAsPrefab(GameObject obj, string prefabName)
    {
        string prefabPath = savePath + prefabName + ".prefab";

        // 检查路径是否存在，如果不存在则创建文件夹
        string folderPath = System.IO.Path.GetDirectoryName(prefabPath);
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        // 检查同名预制体是否已经存在，如果存在则删除
        if (System.IO.File.Exists(prefabPath))
        {
            System.IO.File.Delete(prefabPath);
        }

        // 保存物体为预制体
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);

        Debug.Log("Saved prefab at: " + prefabPath);
    }
    public void CreateItem()
    {



        //生成物品
        GameObject itemObj = Instantiate(itemGameObjectOrigin);
        itemObj.transform.position = new Vector3(0, 3, 0); 
        
        itemObj.name = itemNameField.text;
        Item item = itemObj.GetComponent<Item>();
        //更换Type
        item.info.type = (ItemType)itemIdField.value;
        //更换图片
        Sprite image = objectField_Sprite.value as Sprite;
        for(int i =0;i<itemObj.transform.childCount;i++)
        {
            itemObj.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = image;
        }
        SaveObjectAsPrefab(itemObj, itemNameField.text);
        DestroyImmediate(itemObj);
        //ResourcesSystem设置
        Item_Scriptable newIteminfo = new Item_Scriptable();
        newIteminfo.myName = itemNameField.text;
        newIteminfo.type = (ItemType)itemIdField.value;
        newIteminfo.AnimationSprites.Add(image);
        newIteminfo.MaxAmount = itemAmountFiled.value;
        newIteminfo.MaxDurability = itemDurabilityFiled.value;
        newIteminfo.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(savePath + itemNameField.text + ".prefab");
        ItemText itext = new ItemText();
        itext.text = itemNameField.text;
        itext.color = colorField.value;
        newIteminfo.itemtexts.Add(itext);
        ResourceSystem rSystem = GameObject.Find("Systems").transform.Find("ResourcesSystem").gameObject.GetComponent<ResourceSystem>();
        for (int i = 0; i < rSystem.list.itemlist.Count; i++)
        {
            if (rSystem.list.itemlist[i].myName == newIteminfo.myName)
            {
                rSystem.list.itemlist.Remove(rSystem.list.itemlist[i]);
                break;
            }
        }
        rSystem.list.itemlist.Add(newIteminfo);

        //生成史蒂夫手中的物品
        Transform handTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("SimplePlayer.arma/MAIN/center/Body/Chest/Arm:Right:Upper/Arm:Right:Lower/Arm:Right:Lower_end/Stuffs").transform;
        for (int i = 0; i < handTransform.childCount; i++)
        {
            if (handTransform.GetChild(i).gameObject.name == itemNameField.text)
            {

                DestroyImmediate(handTransform.GetChild(i).gameObject);
                break;
            }
        }
        GameObject g = Instantiate(itemGameObjectOrigin_inHand,handTransform);
        g.name = itemNameField.text;
        for (int i = 0; i < g.transform.childCount; i++)
        {
            g.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = image;
        }
        g.transform.position = handTransform.transform.GetChild(1).transform.position;
        g.SetActive(false);

    }
    public void DeleteItem()
    {
        //删除Prefab
        string prefabPath = savePath + itemNameField.text + ".prefab";
        AssetDatabase.DeleteAsset(prefabPath);
        //删除ResourcesSystem中list
        ResourceSystem rSystem = GameObject.Find("Systems").transform.Find("ResourcesSystem").gameObject.GetComponent<ResourceSystem>();
        foreach(var target in rSystem.list.itemlist)
        {
            if (target.type == (ItemType)itemIdField.value&&target.myName == itemNameField.text)
            {
                rSystem.list.itemlist.Remove(target);
                break;
            }
        }
        //删除手中的物品
        Transform handTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("SimplePlayer.arma/MAIN/center/Body/Chest/Arm:Right:Upper/Arm:Right:Lower/Arm:Right:Lower_end/Stuffs").transform;
       for (int i = 0; i < handTransform.childCount; i++)
        {
            if (handTransform.GetChild(i).name == itemNameField.text)
            {
                DestroyImmediate(handTransform.GetChild(i));
                break;
            }
        }
    }
    
}
