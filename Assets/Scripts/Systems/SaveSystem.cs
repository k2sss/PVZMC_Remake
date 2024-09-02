using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class SaveSystem
{
    public static void SaveUserData(string name, string file, object obj)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data"+MySystem.Instance.nowUserInfo.index.ToString()+"/"+file);
        string ObjectPath = Path.Combine(filePath, name);
        var json = JsonUtility.ToJson(obj);
        if (File.Exists(filePath))
            File.WriteAllText(ObjectPath, json);
        else
        {
            Directory.CreateDirectory(filePath);
            File.WriteAllText(ObjectPath, json);
        }


        
    }
    public static T LoadUserData<T>(string fileName, string file) where T : class
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data" + MySystem.Instance.nowUserInfo.index.ToString() + "/" + file);
        string objPath = Path.Combine(filePath, fileName);
        if (File.Exists(objPath))
        {
            var json = File.ReadAllText(objPath);
            var obj = JsonUtility.FromJson<T>(json);
            
            return obj;
        }
        else
            return null;
    }
    public static void DeleteUserData(int index)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data" + index.ToString());
        Debug.Log(filePath);
        if (Directory.Exists(filePath))
        {
            Directory.Delete(filePath,true);
        }
        else
        {
            Debug.Log("找不到要删除的用户数据");
        }
    }

    public static void Save(string name,string file, object obj)
    {
        string filePath = Path.Combine(Application.persistentDataPath, file);
        string ObjectPath = Path.Combine(filePath,name);
        var json = JsonUtility.ToJson(obj);
        if (File.Exists(filePath))
            File.WriteAllText(ObjectPath, json);
        else
        {
            Directory.CreateDirectory(filePath);
            File.WriteAllText(ObjectPath, json);
        }


        
    }
    public static T Load<T>(string fileName,string file) where T : class
    {
        string filePath = Path.Combine(Application.persistentDataPath, file);
        string objPath = Path.Combine(filePath,fileName);
        if (File.Exists(objPath))
        {
            var json = File.ReadAllText(objPath);
            var obj = JsonUtility.FromJson<T>(json);
           
            return obj;
        }
        else
            return null;
    }

    public static void Save_Data(string name, string file, object obj)
    {
        string filePath = Path.Combine(Application.dataPath, file);
        string ObjectPath = Path.Combine(filePath, name);
        var json = JsonUtility.ToJson(obj);
        if (File.Exists(filePath))
            File.WriteAllText(ObjectPath, json);
        else
        {
            Directory.CreateDirectory(filePath);
            File.WriteAllText(ObjectPath, json);
        }


        Debug.Log("Save Successfully:" + filePath);
    }
    public static T Load_Data<T>(string fileName, string file) where T : class
    {
        string filePath = Path.Combine(Application.dataPath, file);
        string objPath = Path.Combine(filePath, fileName);
        if (File.Exists(objPath))
        {
            var json = File.ReadAllText(objPath);
            var obj = JsonUtility.FromJson<T>(json);
            Debug.Log("Load Successfully");
            return obj;
        }
        else
            return null;

    }
    public static void Delete(string fileName,string file)
    {
        string filePath = Path.Combine(Application.persistentDataPath, file);
        string ObjPath = Path.Combine(file, fileName);
        if (File.Exists(ObjPath))
        {
            File.Delete(ObjPath);
            Debug.Log("Delete Successfully");
        }
    }


}
