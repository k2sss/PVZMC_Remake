using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace MyClass.Tools
{
    public class RanTool
    {
        //取得
        public static int GetWeightResult(int[] weights)
        {
            int sum = weights.Sum();
            int randValue = Random.Range(0,sum);
            for (int i = 0; i < weights.Length; i++)
            {
                randValue -= weights[i];
                if (randValue < 0)
                {
                    return i;
                }
            }
            Debug.Log("无法取得加权随机数下标");
            return -1;
        }
    }
}

