using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileLoadSystem
{
    public static T ResourcesLoad<T>(string loadPath) where T : Object
    {
        return Resources.Load<T>(loadPath);
    }

}
