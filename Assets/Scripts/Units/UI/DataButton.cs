using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataButton : MonoBehaviour
{
    public void Delete()
    {


    }
    public void Load()
    {


    }
    public void OnButtonDown()
    {
        gameObject.GetComponent<Animator>().SetBool("Expand", true);

    }
    public void CancleSelect()
    {
        gameObject.GetComponent<Animator>().SetBool("Expand", false);
    }
}
