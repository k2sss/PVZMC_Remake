using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
public class BlockItemCreator : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private GameObject itemGameObjectOrigin;
    private GameObject itemGameObjectOrigin_inHand;
    private GameObject blockOrigin;
    private Material blockMaterial;
    private ObjectField objectField_Sprite;
    private ObjectField objectField_Material;
    private TextField itemNameField;
    private EnumField ItemtypeField;
    private IntegerField itemDurabilityFiled;
    private IntegerField itemAmountFiled;
    private Button GenerateButton;
    [MenuItem("K2S/BlockItemCreator")]
    public static void ShowExample()
    {
        BlockItemCreator wnd = GetWindow<BlockItemCreator>();
        wnd.titleContent = new GUIContent("BlockItemCreator");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BlockItemCreator.uxml");
        visualTree.CloneTree(root);

        itemGameObjectOrigin = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Item/Item_Block.prefab");
        itemGameObjectOrigin_inHand = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Item/Block_Type_FullSide_InHand.prefab");

        itemNameField = root.Q<TextField>("ItemName");
        ItemtypeField = root.Q<EnumField>("ItemType");
        objectField_Sprite = root.Q<ObjectField>("Sprite");
        objectField_Material = root.Q<ObjectField>("Material");
        itemDurabilityFiled = root.Q<IntegerField>("Durability");
        itemAmountFiled = root.Q<IntegerField>("Amount");
        GenerateButton = root.Q<Button>("Generate");
        GenerateButton.clicked += CreateItem;
     
    }
    public void CreateItem()
    {
        blockMaterial =  objectField_Material.value as Material;

        GameObject itemObj = Instantiate(itemGameObjectOrigin);
        itemObj.transform.position = new Vector3(0, 3, 0);

        itemObj.name = itemNameField.text;
        Item item = itemObj.GetComponent<Item>();
        //更换Type
        item.info.type = (ItemType)ItemtypeField.value;
        //更换图片
        Sprite image = objectField_Sprite.value as Sprite;
        itemObj.GetComponent<Renderer>().sharedMaterial = blockMaterial;
        SaveObjectAsPrefab(itemObj, itemNameField.text, savePath);
        DestroyImmediate(itemObj);
        //ResourcesSystem设置
        Item_Scriptable newIteminfo = new Item_Scriptable();
        newIteminfo.myName = itemNameField.text;
        newIteminfo.type = (ItemType)ItemtypeField.value;
        newIteminfo.AnimationSprites.Add(image);
        newIteminfo.MaxAmount = itemAmountFiled.value;
        newIteminfo.MaxDurability = itemDurabilityFiled.value;
        newIteminfo.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(savePath + itemNameField.text + ".prefab");
             //添加名字显示
        ItemText itext = new ItemText();
        itext.text = itemNameField.text;
        itext.color = Color.white;
        newIteminfo.itemtexts.Add(itext);


        ResourceSystem rSystem = GameObject.Find("Systems").transform.Find("ResourcesSystem").gameObject.GetComponent<ResourceSystem>();
        

        for(int i = 0;i<rSystem.list.itemlist.Count;i++)
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
        for(int i = 0;i<handTransform.childCount;i++)
        {
            if(handTransform.GetChild(i).gameObject.name == itemNameField.text)
            {

                DestroyImmediate(handTransform.GetChild(i).gameObject);
                break;
            }
        }
        
        GameObject g = Instantiate(itemGameObjectOrigin_inHand, handTransform);
        g.name = itemNameField.text;
        g.GetComponent<Renderer>().sharedMaterial = blockMaterial;
        g.transform.position = handTransform.transform.GetChild(1).transform.position;
        g.SetActive(false);

        blockOrigin = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Block/6Side.prefab");
        GameObject b = Instantiate(blockOrigin);
        b.GetComponent<Renderer>().sharedMaterial = blockMaterial;
        SaveObjectAsPrefab(b, itemNameField.text, savePath2);
        DestroyImmediate(b);

    }
    private string savePath = "Assets/Prefabs/Item/"; // 预制体保存路径
    private string savePath2 = "Assets/Prefabs/Block/";

    private void SaveObjectAsPrefab(GameObject obj, string prefabName,string path)
    {
        string prefabPath = path + prefabName + ".prefab";

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
}
