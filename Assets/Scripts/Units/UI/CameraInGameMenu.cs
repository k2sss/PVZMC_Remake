using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInGameMenu : MonoBehaviour
{
   
    public void SetBoolIsScrollTrue()
    {
        gameObject.GetComponent<Animator>().SetBool("IsScroll", true);
    }
}
