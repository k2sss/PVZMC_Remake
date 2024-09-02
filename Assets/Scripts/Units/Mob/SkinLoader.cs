using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkinLoader : MonoBehaviour
{
    private string SkinPath;
    private Mesh[] SlimMeshData;//Alex网格数据
    private Mesh[] MeshData;//史蒂夫网格数据
    private bool Initialized;
    private SkinnedMeshRenderer[] steveRenders = new SkinnedMeshRenderer[2];

    private SkinnedMeshRenderer[] steveRenders2 = new SkinnedMeshRenderer[2];//背包显示的STEVE

    private void Start()
    {
        if(Initialized == false)
        Init();
        ChangePlayerSlim(1);
        LoadPlayerSkin(steveRenders);
        LoadPlayerSkin(steveRenders2);
    }
    public void Init()
    {
        string newpath;
        if (PhoneControlMgr.PhoneControl == false)
            newpath = Application.dataPath + "/skin";
        else
            newpath = Application.persistentDataPath + "/skin";
        if (!File.Exists(newpath))
            Directory.CreateDirectory(newpath);

        
        SlimMeshData = new Mesh[2];
        MeshData = new Mesh[2];
        SlimMeshData[0] = FileLoadSystem.ResourcesLoad<Mesh>("Mesh/SimpleSlimPlayer.Body.Layer1");
        SlimMeshData[1] = FileLoadSystem.ResourcesLoad<Mesh>("Mesh/SimpleSlimPlayer.Body.Layer2");
        MeshData[0] = FileLoadSystem.ResourcesLoad<Mesh>("Mesh/SimplePlayer.Body.Layer1");
        MeshData[1] = FileLoadSystem.ResourcesLoad<Mesh>("Mesh/SimplePlayer.Body.Layer2");

        steveRenders[0] = transform.Find("SimplePlayer.Body.Layer1").GetComponent<SkinnedMeshRenderer>();
        steveRenders[1] = transform.Find("SimplePlayer.Body.Layer2").GetComponent<SkinnedMeshRenderer>();
        steveRenders2[0] = GameObject.FindGameObjectWithTag("Player2").transform.Find("SimplePlayer.Body.Layer1").GetComponent<SkinnedMeshRenderer>();
        steveRenders2[1] = GameObject.FindGameObjectWithTag("Player2").transform.Find("SimplePlayer.Body.Layer2").GetComponent<SkinnedMeshRenderer>();
        //Debug.Log("不存在该文件,已创建新文件夹");
        //if (PhoneControlMgr.Instance.PhoneControl == false)
        //    PathText.text = "皮肤文件路径：" + Application.dataPath + "/skin/steve.png";
        //else
        //    PathText.text = "皮肤文件路径：" + Application.persistentDataPath + "/skin/steve.png";
        Initialized = true;
    }
    public void ChangePlayerSlim(int type)
    {
        if (Initialized == false)
        {
            Init();
        }


        if (type == 0)
        {
            steveRenders[0].sharedMesh = MeshData[0];
            steveRenders[1].sharedMesh = MeshData[1];
            steveRenders2[0].sharedMesh = MeshData[0];
            steveRenders2[1].sharedMesh = MeshData[1];

        }
        else
        {
            steveRenders[0].sharedMesh = SlimMeshData[0];
            steveRenders[1].sharedMesh = SlimMeshData[1];
            steveRenders2[0].sharedMesh = SlimMeshData[0];
            steveRenders2[1].sharedMesh = SlimMeshData[1];
        }

    }

    public void LoadPlayerSkin(SkinnedMeshRenderer[] Mat)
    {
        
        if (PhoneControlMgr.PhoneControl == false)
            SkinPath = Application.dataPath + "/skin/steve.png";
        else
            SkinPath = Application.persistentDataPath + "/skin/steve.png";
        if (File.Exists(SkinPath))
        {
            //Debug.Log("存在该文件");
            Texture2D texture = LoadLocalImage(SkinPath);
            //Sprite SpriteFromTexture = LoadSpriteFromTexture2D(texture);
            texture.Compress(false);
            texture.filterMode = FilterMode.Point;
            for (int i = 0; i < Mat.Length; i++)
            {
                
                Mat[i].material.SetTexture("_BaseMap", texture);
            }
        }
       
    }

    private Texture2D LoadLocalImage(string Imagepath)
    {
        byte[] BytesFromImage = System.IO.File.ReadAllBytes(Imagepath);
        Texture2D loadedTexture = new Texture2D(2, 2);
        loadedTexture.LoadImage(BytesFromImage);
        return loadedTexture;
    }

}
