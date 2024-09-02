using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkGroup
{
    private ModelInfo model;//模型顶点信息
    public ChunkInfo chunkInfo {  set; get; }
    //private GameObject blocks;//物体
    public ChunkBlockInfo chunkblockinfo {
        set
        {

        }
        get
        {
            return GetChunkBlockInfo();
        }
                 }//模型方块储存信息
    private Mesh mesh;
    public GameObject blocks;

    public ChunkGroup() { }
   
    public ChunkGroup(Transform parent)
    {
        mesh = new Mesh();
        model = new ModelInfo();
        chunkInfo = new ChunkInfo();
        blocks = CreateNewGameObject();
        blocks.transform.parent = parent;
    }
    public ChunkBlockInfo GetChunkBlockInfo()
    {
        ChunkBlockInfo go = new ChunkBlockInfo();
        for (int i = 0; i < ChunkInfo.ChunkSize; i++)
            for (int j = 0; j < ChunkInfo.ChunkHeight; j++)
                for (int k = 0; k < ChunkInfo.ChunkSize; k++)
                {
                    BlockInfo info = new BlockInfo(chunkInfo.blocktype[i, j, k]);
                    go.blockinfo.Add(info);
                }
        return go;
    }
    public void SetChunkBlockInfo(ChunkBlockInfo info, int startX, int startZ, bool DrawLeave)
    {
        
        int a = 0;
        for (int i = 0; i < ChunkInfo.ChunkSize; i++)
            for (int j = 0; j < ChunkInfo.ChunkHeight; j++)
                for (int k = 0; k < ChunkInfo.ChunkSize; k++)
                {
                    chunkInfo.blocktype[i, j, k] = info.blockinfo[a].t;
                    a++;
                }
        blocks.transform.position = new Vector3(startX, 0, startZ);
        if (DrawLeave == false)
        {
            blocks.GetComponent<MeshRenderer>().sharedMaterial = FileLoadSystem.ResourcesLoad<Material>("blockMaterials");
            StartDraw(false);
        }
        else
        {
            blocks.GetComponent<MeshRenderer>().sharedMaterial = FileLoadSystem.ResourcesLoad<Material>("Block_wind");
            StartDraw(true);
        }


        RefreshMesh();
    }
    private void SetMesh(Vector3[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            this.model.verticles.Add((vertices[i]));
        }
        this.model.triangles.Add(model.verticles.Count - 4);
        this.model.triangles.Add(model.verticles.Count - 3);
        this.model.triangles.Add(model.verticles.Count - 2);
        this.model.triangles.Add(model.verticles.Count - 2);
        this.model.triangles.Add(model.verticles.Count - 3);
        this.model.triangles.Add(model.verticles.Count - 1);
        //
    }

    public void RefreshMesh()
    {

        mesh.Clear();
        mesh.vertices = model.verticles.ToArray();
        mesh.triangles = model.triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.uv = model.uvs.ToArray();
        blocks.GetComponent<MeshCollider>().sharedMesh = mesh;

    }

    public void SetBlockType(BlockType blocktype, int Perlinstrength, int PerLinSize, int startX, int startZ)//噪声生成测试 
    {
        
        for (int i = startX; i < ChunkInfo.ChunkSize + startX; i++)
            for (int k = startZ; k < ChunkInfo.ChunkSize + startZ; k++)
                for (int j = 0; j < Perlinstrength * Mathf.PerlinNoise(i / (float)PerLinSize, (float)k / PerLinSize); j++)
                {
                   
                   chunkInfo.blocktype[i - startX, j, k - startZ] = blocktype;
                    
                }
        blocks.transform.position = new Vector3(startX, 0, startZ);
      
        blocks.GetComponent<MeshRenderer>().sharedMaterial = FileLoadSystem.ResourcesLoad<Material>("blockMaterials");
        StartDraw();
        RefreshMesh();
    }

    public void StartDraw()//开始根据信息来绘制
    {
        //清空
        model.triangles.Clear();
        model.uvs.Clear();
        model.verticles.Clear();

        for (int i = 0; i < ChunkInfo.ChunkSize; i++)
            for (int j = 0; j < ChunkInfo.ChunkHeight; j++)
                for (int k = 0; k < ChunkInfo.ChunkSize; k++)
                {
                    if (chunkInfo.blocktype[i, j, k] != BlockType.air)
                        DrawABlock(i, j, k);
                }
    }
    public void StartDraw(bool IsLeave)//开始根据信息来绘制
    {
        //清空
        model.triangles.Clear();
        model.uvs.Clear();
        model.verticles.Clear();

        for (int i = 0; i < ChunkInfo.ChunkSize; i++)
            for (int j = 0; j < ChunkInfo.ChunkHeight; j++)
                for (int k = 0; k < ChunkInfo.ChunkSize; k++)
                {
                    if (IsLeave == true)
                    {
                        if (chunkInfo.blocktype[i, j, k] != BlockType.air)
                            if (IsLeaveType(i,j,k))
                                DrawABlock(i, j, k);

                    }
                    else
                    {
                        if (chunkInfo.blocktype[i, j, k] != BlockType.air)
                            if (!IsLeaveType(i, j, k))
                                DrawABlock(i, j, k);
                    }

                }
    }
    private bool IsLeaveType(int i,int j,int k)
    {
        if (chunkInfo.blocktype[i, j, k] == BlockType.oak_leave_empty || chunkInfo.blocktype[i, j, k] == BlockType.gloomyWodd_Leave)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CreateANewBlock(int x, int y, int z, BlockType type)
    {
        chunkInfo.blocktype[x, y, z] = type;
        StartDraw();
        RefreshMesh();
    }
    public void ReplaceBlockData(int x, int y, int z, BlockType type)
    {
        chunkInfo.blocktype[x, y, z] = type;
    }
   
    public void CreateANewBlock(int x, int y, int z, BlockType type, bool DrawLeave)
    {
        if (DrawLeave == true)
        {
            chunkInfo.blocktype[x, y, z] = type;
            StartDraw(true);
            RefreshMesh();
        }
    }
    private void DrawABlock(int x, int y, int z)//绘制一个方块
    {

        if (CheckBlocks(x, y, z, BlockUVToward.forward))
        {
            DrawOneSide(x, y, z, BlockUVToward.forward);
        }

        if (CheckBlocks(x, y, z, BlockUVToward.backward))
        {
            DrawOneSide(x, y, z, BlockUVToward.backward);

        }
        if (CheckBlocks(x, y, z, BlockUVToward.left))
        {
            DrawOneSide(x, y, z, BlockUVToward.left);
        }
        if (CheckBlocks(x, y, z, BlockUVToward.right))
        {
            DrawOneSide(x, y, z, BlockUVToward.right);
        }
        if (CheckBlocks(x, y, z, BlockUVToward.up))
        {
            DrawOneSide(x, y, z, BlockUVToward.up);
        }
        if (CheckBlocks(x, y, z, BlockUVToward.down))
        {
            DrawOneSide(x, y, z, BlockUVToward.down);
        }
    }
    private void DrawOneSide(int x, int y, int z, BlockUVToward toward)
    {
        Vector3[] _verticles = new Vector3[1];
        switch (toward)
        {
            case BlockUVToward.forward:

                _verticles = new Vector3[]
                {

            new Vector3(x,y,z),
            new Vector3(x,y+1,z),
            new Vector3(x+1,y,z),
            new Vector3(x+1,y+1,z),
                };

                break;
            case BlockUVToward.backward:

                _verticles = new Vector3[]
                {

               new Vector3(x +1,y,z+1),
                             new Vector3(x +1,y+1,z +1),
                               new Vector3(x,y,z+1),
                                 new Vector3(x,y+1,z+1)
                };

                break;
            case BlockUVToward.left:

                _verticles = new Vector3[]
                {

          new Vector3(x,y,z+1),
                              new Vector3(x,y+1,z+1),
                                new Vector3(x,y,z),
                                  new Vector3(x,y+1,z)
                };

                break;
            case BlockUVToward.right:

                _verticles = new Vector3[]
                {

            new Vector3(x+1,y,z),
                                  new Vector3(x+1,y+1,z),
                                    new Vector3(x+1,y,z+1),
                                      new Vector3(x+1,y+1,z+1)
                };

                break;
            case BlockUVToward.up:

                _verticles = new Vector3[]
                {

            new Vector3(x,y+1,z+1),
            new Vector3(x+1,y+1,z+1),
            new Vector3(x,y+1,z),
                            new Vector3(x+1,y+1,z),
                };

                break;
            case BlockUVToward.down:

                _verticles = new Vector3[]
                {

          new Vector3(x+1,y,z),
                              new Vector3(x+1,y,z+1),
                                new Vector3(x,y,z),
                                  new Vector3(x,y,z+1)
                };

                break;


        };
        SetMesh(_verticles);
        Vector2[] uvs = new Vector2[1];
        BlockUV.Getuv(ref uvs, chunkInfo.blocktype[x, y, z], toward);
        for (int i = 0; i < uvs.Length; i++)
        {
            this.model.uvs.Add(uvs[i]);
        }

    }
    private bool CheckBlocks(int x, int y, int z, BlockUVToward toward)//检查一个方块四周没有另外一个方块
    {
        try
        {
            switch (toward)
            {
                case BlockUVToward.forward:
                    if (getTransparentBlockType(x, y, z - 1) == BlockType.air)
                    {
                        return true;
                    }
                    break;
                case BlockUVToward.backward:
                    if (getTransparentBlockType(x, y, z + 1) == BlockType.air)
                    {
                        return true;
                    }
                    break;
                case BlockUVToward.left:
                    if (getTransparentBlockType(x - 1, y, z) == BlockType.air)
                    {
                        return true;
                    }
                    break;
                case BlockUVToward.right:
                    if (getTransparentBlockType(x + 1, y, z) == BlockType.air)
                    {
                        return true;
                    }
                    break;
                case BlockUVToward.up:
                    if (getTransparentBlockType(x, y + 1, z) == BlockType.air)
                    {
                        return true;
                    }
                    break;
                case BlockUVToward.down:
                    if (getTransparentBlockType(x, y - 1, z) == BlockType.air)
                    {
                        return true;
                    }
                    break;
            }
        }
        catch
        {
            return true;
        }
        return false;



    }
    private BlockType getTransparentBlockType(int x, int y, int z)
    {
        BlockType go = BlockType.dirt;
        BlockType a = chunkInfo.blocktype[x, y, z];
        if (a == BlockType.air ||
             a == BlockType.oak_leave_empty||a ==BlockType.gloomyWodd_Leave
            )
        {
            go = BlockType.air;
        }
        return go;
    }


    private GameObject CreateNewGameObject()
    {
        GameObject go = new GameObject();
        go.AddComponent(typeof(MeshFilter));
        go.AddComponent(typeof(MeshCollider));
        go.AddComponent(typeof(MeshRenderer));
        go.name = "trunk";
        go.layer = 3;
        go.GetComponent<MeshCollider>().material = Resources.Load<PhysicMaterial>("Smooth");
        go.GetComponent<MeshFilter>().mesh = mesh;
        return go;
    }


}
