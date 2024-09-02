using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Intro_Event1 : MonoBehaviour
{
    TextPrinter printer;
    AsyncOperation go;
    private void Start()
    {
        printer = GetComponent<TextPrinter>();
        printer.TalkOverActions[0] += PreLoad;
        printer.TalkOverActions[1] += Load;
    }
    public void PreLoad()
    {
       go = SceneMgr.Instance.LoadAsync("Select1_Intro");
       //Debug.Log("Preloading......");
       go.allowSceneActivation = false;  
    }
   public void Load()
    {
        //Debug.Log(go.progress);
        go.allowSceneActivation = true;
    }



}
