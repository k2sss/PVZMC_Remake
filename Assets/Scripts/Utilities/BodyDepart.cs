using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDepart : MonoBehaviour
{
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        for(int i =0;i<transform.childCount;i++)
        {
            Rigidbody rb = transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.5f;
            rb.velocity = new Vector3(2, 3, 0);
            MeshCollider c = transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
            c.convex = true;
        }
        MonoController.Instance.Invoke(2, () => Destroy(gameObject));
    }
}
