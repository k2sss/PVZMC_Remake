using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManagerPersistent <T>: BaseManager<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}
