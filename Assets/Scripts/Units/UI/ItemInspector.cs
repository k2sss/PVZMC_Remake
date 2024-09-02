using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInspector : MonoBehaviour
{
    public GameObject TextPrefab;
    public List<GameObject> childs = new List<GameObject>();
    public static ItemInspector Instance;
    private Image _image;
    private void Awake()
    {
        Instance = this;
        _image = GetComponent<Image>();
    }
    private void Start()
    {
        CheckItem(ItemType.Nothing);
    }
    private void Update()
    {
        FollowPointer();
    }
    public void CheckItem(ItemType type)
    {
        if (type != ItemType.Nothing)
        {
            _image.enabled = true;
            for (int i = 0; i < childs.Count; i++)
            {
                Destroy(childs[i].gameObject);
            }
            childs.Clear();
            Item_Scriptable itemCreateinfo = ResourceSystem.Instance.GetItem(type);
            for (int i = 0; i < itemCreateinfo.itemtexts.Count; i++)
            {
                GameObject a = Instantiate(TextPrefab, transform);
                a.GetComponent<Text>().text = itemCreateinfo.itemtexts[i].text;
                a.GetComponent<Text>().color = itemCreateinfo.itemtexts[i].color;
                childs.Add(a);
            }
        }
        else
        {
            _image.enabled = false;
            for (int i = 0; i < childs.Count; i++)
            {
                Destroy(childs[i].gameObject);
            }

            childs.Clear();
        }
    }
    private void FollowPointer()
    {
        transform.position = Input.mousePosition + new Vector3(30,-10,0);
    }
    private void OnEnable()
    {
        Instance = this;
    }
}
