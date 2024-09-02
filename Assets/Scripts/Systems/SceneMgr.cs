using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SceneMgr : BaseManager<SceneMgr>
{

    public  AsyncOperation LoadAsync(string sceneName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AsyncOperation LoadAsy = SceneManager.LoadSceneAsync(sceneName);
            return LoadAsy;
        }
        else
        {
            UIMgr.Instance.GetUIObject("BlackMask").GetComponent<Animator>().SetBool("Out", true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            MonoController.Instance.InvokeUnScaled(1, () => operation.allowSceneActivation = true);
            return operation;
        }

    }
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    public static string GetSceneNameStatic()
    {
        return SceneManager.GetActiveScene().name;
    }
}
