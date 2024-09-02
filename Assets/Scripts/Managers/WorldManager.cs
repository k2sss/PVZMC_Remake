using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.IO;

public class WorldManager : BaseManager<WorldManager>
{
    public bool NeedMapData;
    public ChunkGroup[,] chunkGroups;
    public ChunkGroup[,] chunkGroups_leave;
    public bool ShowMapInfomation;
    public bool ObjectCreateMode;
    [HideInInspector] public int ChunkSize, PerlinSize, PerlinStrength;
    [HideInInspector] public BlockType type;
    [HideInInspector] public int Strength;
    [HideInInspector] public GameObject Obj;
    public int Size;
    public int Height;
    public int RandomSize;
    public BlockType paintType;
    public string SaveAndLoadName;
    public BackInfo backInfo;
    private bool Able2LoadWorld = true;
    private static HashSet<ChunkGroup> chunkGroupSet = new HashSet<ChunkGroup>();

    protected override void Awake()
    {
        base.Awake();
        chunkGroupSet.Clear();
    }
    public static void UpdateWorldInfo()
    {
        if (chunkGroupSet == null)
            return;

        foreach (var item in chunkGroupSet)
        {
            item.StartDraw();
            item.RefreshMesh();
        }
        chunkGroupSet.Clear();
    }
    private void Start()
    {
        if (Able2LoadWorld == true)
            Load();
    }
    public void UnAbleLoad()
    {
        Able2LoadWorld = false;
    }



    [ContextMenu("CreateAWorld")]
    public void CreateAWolrd()
    {
        DeleteWorld();
        backInfo = new BackInfo();
        chunkGroups = new ChunkGroup[ChunkSize, ChunkSize];
        chunkGroups_leave = new ChunkGroup[ChunkSize, ChunkSize];
        for (int i = 0; i < ChunkSize; i++)
            for (int j = 0; j < ChunkSize; j++)
            {

                chunkGroups[i, j] = new ChunkGroup(transform);
                chunkGroups[i, j].SetBlockType(type, PerlinStrength, PerlinSize, i * ChunkInfo.ChunkSize, j * ChunkInfo.ChunkSize);
            }
        for (int i = 0; i < ChunkSize; i++)
            for (int j = 0; j < ChunkSize; j++)
            {
                chunkGroups_leave[i, j] = new ChunkGroup(transform);
            }
    }
    [ContextMenu("Delete")]
    public void DeleteWorld()
    {
        int num = transform.childCount;
        for (int i = 0; i < num; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject, true);
        }
        chunkGroups = new ChunkGroup[ChunkSize, ChunkSize];
    }
    public bool CreateANewBlock(int x, int y, int z, BlockType type)
    {

        int chunkX = x / (ChunkInfo.ChunkSize);
        int chunkZ = z / (ChunkInfo.ChunkSize);
        int realx = x % (ChunkInfo.ChunkSize);
        int realz = z % (ChunkInfo.ChunkSize);
        try
        {
            chunkGroups[chunkX, chunkZ].CreateANewBlock(realx, y, realz, type);
            chunkGroups_leave[chunkX, chunkZ].CreateANewBlock(realx, y, realz, type, true);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public ChunkGroup ReplaceABlock_Buffer(int x, int y, int z, BlockType type)
    {

        int chunkX = x / (ChunkInfo.ChunkSize);
        int chunkZ = z / (ChunkInfo.ChunkSize);
        int realx = x % (ChunkInfo.ChunkSize);
        int realz = z % (ChunkInfo.ChunkSize);
        try
        {
            if (chunkGroups[chunkX, chunkZ].chunkInfo.blocktype[realx, y, realz] != BlockType.air)
            {
                chunkGroups[chunkX, chunkZ].ReplaceBlockData(realx, y, realz, type);
                chunkGroupSet.Add(chunkGroups[chunkX, chunkZ]);
                return chunkGroups[chunkX, chunkZ];
            }
           
        }
        catch
        {
            Debug.Log("³öÏÖ´íÎó");
        }
        return null;
    }
    public bool ReplaceABlock(int x, int y, int z, BlockType type)
    {

        int chunkX = x / (ChunkInfo.ChunkSize);
        int chunkZ = z / (ChunkInfo.ChunkSize);
        int realx = x % (ChunkInfo.ChunkSize);
        int realz = z % (ChunkInfo.ChunkSize);
        try
        {
            if (chunkGroups[chunkX, chunkZ].chunkInfo.blocktype[realx, y, realz] != BlockType.air)
            {
                chunkGroups[chunkX, chunkZ].CreateANewBlock(realx, y, realz, type);

            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public BlockType GetType(int x, int y, int z)
    {

        int chunkX = x / (ChunkInfo.ChunkSize);
        int chunkZ = z / (ChunkInfo.ChunkSize);
        int realx = x % (ChunkInfo.ChunkSize);
        int realz = z % (ChunkInfo.ChunkSize);

        return chunkGroups[chunkX, chunkZ].chunkInfo.blocktype[realx, y, realz];
    }
#if UNITY_EDITOR
    public void Save()
    {
        WorldInfo info = new WorldInfo();
        for (int i = 0; i < ChunkSize; i++)
            for (int j = 0; j < ChunkSize; j++)
            {
                ChunkBlockInfo go = chunkGroups[i, j].GetChunkBlockInfo();
                info.chunkBlockInfos.Add(go);
            }
        WorldInfo_Scriptable SworldInfo = ScriptableObject.CreateInstance<WorldInfo_Scriptable>();
        SworldInfo.chunkBlockInfos = info.chunkBlockInfos;
        string path = "Assets/Resources/SciptableObjects/MapInfo_Scriptable/" + SaveAndLoadName + ".asset";

        AssetDatabase.CreateAsset(SworldInfo, path);
        AssetDatabase.SaveAssets();

    }
#endif
    public void Load()
    {
        DeleteWorld();
        backInfo = new BackInfo();
        WorldInfo info = new WorldInfo();
        info.chunkBlockInfos = Resources.Load<WorldInfo_Scriptable>("SciptableObjects/MapInfo_Scriptable/" + SaveAndLoadName).chunkBlockInfos;
        // WorldInfo info = SaveSystem.Load_Data<WorldInfo>(SaveAndLoadName, "Resources/SciptableObjects/MapInfo");
        chunkGroups = new ChunkGroup[ChunkSize, ChunkSize];
        chunkGroups_leave = new ChunkGroup[ChunkSize, ChunkSize];
        int a = 0;
        for (int i = 0; i < ChunkSize; i++)
        {
            for (int j = 0; j < ChunkSize; j++)
            {

                chunkGroups[i, j] = new ChunkGroup(transform);
                chunkGroups[i, j].SetChunkBlockInfo(info.chunkBlockInfos[a], i * ChunkInfo.ChunkSize, j * ChunkInfo.ChunkSize, false);
                a++;
            }
        }
        a = 0;
        for (int i = 0; i < ChunkSize; i++)
        {
            for (int j = 0; j < ChunkSize; j++)
            {
                chunkGroups_leave[i, j] = new ChunkGroup(transform);
                chunkGroups_leave[i, j].SetChunkBlockInfo(info.chunkBlockInfos[a], i * ChunkInfo.ChunkSize, j * ChunkInfo.ChunkSize, true);
                a++;
            }
        }

    }

    public void Back()
    {
        if (backInfo.stepLists.Count > 0)
            for (int i = 0; i < backInfo.stepLists[backInfo.stepLists.Count - 1].stepList.Count; i++)
            {
                CreateANewBlock(backInfo.stepLists[backInfo.stepLists.Count - 1].stepList[i].x,
                backInfo.stepLists[backInfo.stepLists.Count - 1].stepList[i].y,
                backInfo.stepLists[backInfo.stepLists.Count - 1].stepList[i].z, backInfo.stepLists[backInfo.stepLists.Count - 1].stepList[i].t);
            }
        if (backInfo.stepLists.Count > 0)
            backInfo.stepLists.Remove(backInfo.stepLists[backInfo.stepLists.Count - 1]);
    }
    public void ExpandChunk(int fromChunckSize, int toChunkSize)
    {
        WorldInfo info = new WorldInfo();
        info.chunkBlockInfos = Resources.Load<WorldInfo_Scriptable>("SciptableObjects/MapInfo_Scriptable/" + SaveAndLoadName).chunkBlockInfos;
        chunkGroups = new ChunkGroup[toChunkSize, toChunkSize];
        chunkGroups_leave = new ChunkGroup[toChunkSize, toChunkSize];
        int a = 0;
        for (int i = 0; i < toChunkSize; i++)
        {
            for (int j = 0; j < toChunkSize; j++)
            {

                chunkGroups[i, j] = new ChunkGroup(transform);

                chunkGroups[i, j].SetChunkBlockInfo(info.chunkBlockInfos[a], i * ChunkInfo.ChunkSize, j * ChunkInfo.ChunkSize, false);
                a++;
            }
        }
        a = 0;
        for (int i = 0; i < ChunkSize; i++)
        {
            for (int j = 0; j < ChunkSize; j++)
            {
                chunkGroups_leave[i, j] = new ChunkGroup(transform);
                chunkGroups_leave[i, j].SetChunkBlockInfo(info.chunkBlockInfos[a], i * ChunkInfo.ChunkSize, j * ChunkInfo.ChunkSize, true);
                a++;
            }
        }


    }



}
