using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInfo
{
    public static int ChunkSize = 16;
    public static int ChunkHeight = 64;
    public BlockType[,,] blocktype;

    public ChunkInfo()
    {
        blocktype = new BlockType[ChunkSize, ChunkHeight, ChunkSize];
    }

}
