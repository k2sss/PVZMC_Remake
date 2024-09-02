using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemCreator : MonoBehaviour
{
    [SerializeField]
    private int ItemID;
    [SerializeField]
    private string ItemName ="NoName";
    [SerializeField]
    private int durability = 1;
    [SerializeField]
    private int maxAmount = 64;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Color textColor = Color.white ;
    private string savePath = "Assets/Prefabs/Item/"; // Ԥ���屣��·��

    [SerializeField]
    private ItemText[] descriptions;
#if UNITY_EDITOR
    private void SaveObjectAsPrefab(GameObject obj, string prefabName)
    {


        string prefabPath = savePath + prefabName + ".prefab";

        // ���·���Ƿ���ڣ�����������򴴽��ļ���
        string folderPath = System.IO.Path.GetDirectoryName(prefabPath);
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        // ���ͬ��Ԥ�����Ƿ��Ѿ����ڣ����������ɾ��
        if (System.IO.File.Exists(prefabPath))
        {
            System.IO.File.Delete(prefabPath);
        }

        // ��������ΪԤ����
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);

        Debug.Log("Saved prefab at: " + prefabPath);

    }
    public void CreateItem()
    {

        //������Ʒ
        GameObject itemObj = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Item/Origin.prefab",typeof(GameObject)) as GameObject);
        itemObj.transform.position = new Vector3(0, 3, 0);
        itemObj.name = ItemName;
        Item item = itemObj.GetComponent<Item>();
        //����Type
        item.info.type = (ItemType)ItemID;
        //����ͼƬ
       
        for (int i = 0; i < itemObj.transform.childCount; i++)
        {
            itemObj.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        SaveObjectAsPrefab(itemObj, ItemName);
        DestroyImmediate(itemObj);
        //ResourcesSystem����
        Item_Scriptable newIteminfo = new Item_Scriptable();
        newIteminfo.myName = ItemName;
        newIteminfo.type = (ItemType)ItemID;
        newIteminfo.AnimationSprites.Add(sprite);
        newIteminfo.MaxAmount = maxAmount;
        newIteminfo.MaxDurability = durability;
        newIteminfo.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(savePath + ItemName + ".prefab");
        ItemText itext = new ItemText();
        itext.text = ItemName;
        itext.color = textColor;
        newIteminfo.itemtexts.Add(itext);
        for(int i = 0;i<descriptions.Length;i++)
        {
            newIteminfo.itemtexts.Add(descriptions[i]);
        }

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

        //����ʷ�ٷ����е���Ʒ
        Transform handTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("SimplePlayer.arma/MAIN/center/Body/Chest/Arm:Right:Upper/Arm:Right:Lower/Arm:Right:Lower_end/Stuffs").transform;
        for (int i = 0; i < handTransform.childCount; i++)
        {
            if (handTransform.GetChild(i).gameObject.name == ItemName)
            {

                DestroyImmediate(handTransform.GetChild(i).gameObject);
                break;
            }
        }
        GameObject g = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Item/OriginInhand.prefab", typeof(GameObject)) as GameObject, handTransform);
        g.name =ItemName;
        for (int i = 0; i < g.transform.childCount; i++)
        {
            g.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        g.transform.position = handTransform.transform.GetChild(1).transform.position;
        g.SetActive(false);

    }
#endif
}
