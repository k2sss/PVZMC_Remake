using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUV
{
    //我需要的功能：给特殊方块帖UV，储存6个面应给的UV
    private static int uvscale = 256 / 16;
    
    public static void  Getuv(ref Vector2[] uv,BlockType blocktype,BlockUVToward toward)
    {

        switch (blocktype)
        {
            case BlockType.dirt:
                SetUv(ref uv, 0, 0);
                break;
            case BlockType.stone:
                SetUv(ref uv, 3, 0);
                break;
            case BlockType.grass_block:
                if(toward == BlockUVToward.up)
                SetUv(ref uv, 1, 0);
                else if(toward == BlockUVToward.down)
                    SetUv(ref uv, 0, 0);
                else
                    SetUv(ref uv, 2, 0);
                 break;
            case BlockType.grass_block_light:
                if (toward == BlockUVToward.up)
                    SetUv(ref uv, 3, 1);
                else if (toward == BlockUVToward.down)
                    SetUv(ref uv, 0, 0);
                else
                    SetUv(ref uv, 2, 0);
                break;
             
            case BlockType.oak_log:
                if (toward == BlockUVToward.up|| toward == BlockUVToward.down)
                    SetUv(ref uv, 5, 0);
                else
                    SetUv(ref uv, 4, 0);
                break;
            case BlockType.oak_leave_filled:
                SetUv(ref uv, 6, 0);
                break;
            case BlockType.oak_leave_empty:
                SetUv(ref uv, 7, 0);
                break;
            case BlockType.crafting_table:
                if (toward == BlockUVToward.up || toward == BlockUVToward.down)
                    SetUv(ref uv, 1, 1);
                else if (toward == BlockUVToward.left || toward == BlockUVToward.right)
                    SetUv(ref uv, 0, 1);
                else if (toward == BlockUVToward.forward || toward == BlockUVToward.backward)
                    SetUv(ref uv, 2, 1);
                break;
            case BlockType.obisidian:
                SetUv(ref uv, 10, 0);
                break;
            case BlockType.obisidian_cry:
                SetUv(ref uv, 11, 0);
                break;
            case BlockType.Nether_block:
                SetUv(ref uv, 8, 0);
                break;
            case BlockType.Nether_block_grass:
                if(toward == BlockUVToward.up)
                SetUv(ref uv, 9, 1);
                else if(toward != BlockUVToward.down)
                    SetUv(ref uv, 9, 0);
                else
                    SetUv(ref uv, 8, 0);
                break;
            case BlockType.White:
                SetUv(ref uv, 4, 1);
                break;
            case BlockType.Cobblestone:
                SetUv(ref uv, 13, 0);
                break;
            case BlockType.Cobblestone_mossy:
                SetUv(ref uv, 14, 0);
                break;
            case BlockType.Sand:
                SetUv(ref uv, 5, 1);
                break;
            case BlockType.SandStone:
                if (toward == BlockUVToward.up)
                    SetUv(ref uv, 7, 1);
                else if (toward == BlockUVToward.down)
                    SetUv(ref uv, 5, 2);
                else
                    SetUv(ref uv, 6, 1);
                break;
            case BlockType.SandStone_smooth:
                if (toward == BlockUVToward.up)
                    SetUv(ref uv, 7, 1);
                else if (toward == BlockUVToward.down)
                    SetUv(ref uv, 5, 2);
                else
                    SetUv(ref uv, 8, 1);
                break;

            case BlockType.sandStone_chiseled:
                if (toward == BlockUVToward.up)
                    SetUv(ref uv, 7, 1);
                else if (toward == BlockUVToward.down)
                    SetUv(ref uv, 5, 2);
                else
                    SetUv(ref uv, 6, 2);
                break;
            case BlockType.sandStone_1:
                SetUv(ref uv, 7, 2);
                break;
            case BlockType.sandStone_2:
                SetUv(ref uv, 8, 2);
                break;
            case BlockType.sandStone_brick:
                SetUv(ref uv, 9, 2);
                break;
            case BlockType.fleshdirt:
                SetUv(ref uv, 10, 2);
                break;
            case BlockType.fleshBlock:
                SetUv(ref uv, 12, 1);
                break;
            case BlockType.fleshgrass:
                if (toward == BlockUVToward.up)
                {
                    SetUv(ref uv, 10, 1);
                }
                else if(toward == BlockUVToward.down)
                {
                    SetUv(ref uv, 10, 2);
                }
                else
                {
                    SetUv(ref uv, 11, 2);
                }
                break;
            case BlockType.gloomyWood:
                if(toward == BlockUVToward.up|| toward == BlockUVToward.down)
                SetUv(ref uv, 14, 1);
                else
                    SetUv(ref uv, 13, 1);
                break;
            case BlockType.gloomyWodd_Leave:
                SetUv(ref uv, 15, 0);
                break;
            case BlockType.fleshOre:
                SetUv(ref uv, 11, 1);
                break;
            case BlockType.fleshSand:
                SetUv(ref uv, 12, 0);
                break;
            case BlockType.fleshBone1:
                SetUv(ref uv, 12, 2);
                break;
            case BlockType.fleshBone2:
                SetUv(ref uv, 13, 2);
                break;
            case BlockType.fleshBone3:
                SetUv(ref uv, 14, 2);
                break;

            default:
                SetUv(ref uv, uvscale-1, uvscale-1);
                break;
        }
    }
    private static void SetUv(ref Vector2[] uv,int x,int y)
    {
        uv = new Vector2[]
        {
            new Vector2(x/(float)uvscale,-y/(float)uvscale+1-1f/uvscale),
            new Vector2(x/(float)uvscale,-y/(float)uvscale+1),
            new Vector2(x/(float)uvscale + 1f/uvscale,-y/(float)uvscale+1 - 1f/uvscale),
            new Vector2(x/(float)uvscale + 1f/uvscale,-y/(float)uvscale+1),
        };
    }


}
