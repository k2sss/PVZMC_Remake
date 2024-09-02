using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ItemRigidbody))]
public class Item : MonoBehaviour
{
    public ItemInfo info;
    [HideInInspector]public SpriteRenderer[] _renderers;
    public bool notAble;
    private void Awake()
    {
        gameObject.tag = "Item";
    }
    private void Start()
    {
        if(notAble == false)
        info.Durability = ResourceSystem.Instance.GetItem(info).MaxDurability;
    }
    public void OnThrow()
    {
        Disable();
        Invoke("Enable", 3);
    }
    public void Enable()
    {
        notAble = false;
    }
    public void Disable()
    {
        notAble = true;
    }
    public void ChangeSprite(Sprite sprite)
    {
        GetChildRenderer();
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].sprite = sprite;
        }
    }
    public void GetChildRenderer()
    {
        _renderers = new SpriteRenderer[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _renderers[i] = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (notAble == false&&other.gameObject.CompareTag("Player"))
        {
            
            int rest = InventoryManager.Instance.AddItems(info);
            info.Count = rest;
            
            if (info.Count <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
