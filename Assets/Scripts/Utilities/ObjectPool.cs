using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool
{
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;

        if (pool == null) //当场景没有对象池时（第一次进入游戏或者切换了场景），新建一个对象池游戏物品并清空字典
        {
            pool = new GameObject("ObjectPool");
            objectPool = new Dictionary<string, Queue<GameObject>>();
        }
        //如果池子里没有该物品

        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            //实例化它，加入队列
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            //设置到相对于的子物品下，方便管理
            _object.transform.SetParent(childPool.transform);
        }
        //从队列中提取对象，返回
        _object = objectPool[prefab.name].Dequeue();
        _object.SetActive(true);
        return _object;

    }
    public void PushObject(GameObject prefab)
    {
        //通过Instantiate实例化的物品都带有（Clone）后缀，我们将其去除再存储
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(_name))
            objectPool.Add(_name, new Queue<GameObject>());
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}