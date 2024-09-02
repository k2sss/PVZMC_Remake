using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class CraftInfo
{
    public string name;
    public ItemType[] types;
    [Header("OutPut")]
    public ItemType OutPutType;
    public int OutPutNum;
    public int OutPutDurability = 1;

    public CraftInfo(string carftName, int[] sourceTypes,int outPutType,int outPutNum)
    {
        name = carftName;
        types = sourceTypes.Select((s) => (ItemType)s).ToArray();
        this.OutPutType = (ItemType)outPutType;
        this.OutPutNum = outPutNum;
    }
}
