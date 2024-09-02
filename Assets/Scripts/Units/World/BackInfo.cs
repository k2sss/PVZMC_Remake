using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackInfo
{
    public List<Step> stepLists = new List<Step>();
}
public class Step
{
    public List <LittleStep> stepList = new List<LittleStep>();
}
public class LittleStep
{
    public int x;
    public int y;
    public int z;
    public BlockType t;
    public LittleStep(int x, int y, int z, BlockType t)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.t = t;
    }
}