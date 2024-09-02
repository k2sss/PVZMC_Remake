using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WorldInfo
{
  // public List<ModelInfo> blocksinfo = new List<ModelInfo>();
   public List<ChunkBlockInfo> chunkBlockInfos = new List<ChunkBlockInfo>();
}

[System.Serializable]
public class ModelInfo
{
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Vector3> verticles = new List<Vector3>();
}
[System.Serializable]
public class ChunkBlockInfo
{
   public List<BlockInfo> blockinfo = new List<BlockInfo>();
}

[System.Serializable]
public class BlockInfo
{
    public BlockType t;

    public BlockInfo(BlockType t)
    {
        this.t = t;
    }

}