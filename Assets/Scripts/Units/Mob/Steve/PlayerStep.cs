using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStep : MonoBehaviour
{
    [HideInInspector]public AudioSource selfSource;
    private void Awake()
    {
        selfSource = GetComponent<AudioSource>();
    }


    public void RayCheckGround()
    {
        if (PlayerMoveController.Instance.IsMove()&& !PlayerMoveController.Instance.IsJump())
        {
            float raylength = 2f;
            Vector3 rayorigin = transform.position + new Vector3(0, 0.1f, 0);
            Ray ray = new Ray(rayorigin, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raylength, 1 << 3))
            {
                int x = (int)hit.point.x;
                int y = (int)hit.point.y - 1;
                int z = (int)hit.point.z; 
                BlockType blockType = WorldManager.Instance.GetType(x, y, z);
                //Debug.Log(blockType);
                switch (blockType)
                {
                    case BlockType.sandStone_1:
                    case BlockType.sandStone_2:
                    case BlockType.sandStone_chiseled:
                    case BlockType.sandStone_brick:
                    case BlockType.SandStone_smooth:
                    case BlockType.SandStone:
                    case BlockType.stone:
                    case BlockType.Cobblestone:
                    case BlockType.Cobblestone_mossy:
                    case BlockType.fleshOre:
                    case BlockType.fleshBlock:
                    case BlockType.obisidian_cry:
                    case BlockType.obisidian:
                    case BlockType.fleshBone1:
                    case BlockType.fleshBone2:
                    case BlockType.fleshBone3:

                        PlayRandomSound("StoneWalk");
                        break;
                    case BlockType.Sand:
                    case BlockType.fleshSand:
                        PlayRandomSound("SandWalk");
                        break;

                    default:
                        PlayRandomSound("GrassWalk");
                        break;


                }
            }
        }
        
    }
    public void PlayRandomSound(string clipName)
    {
        AudioClip[] clips = SoundSystem.Instance.GetAudioClips(clipName);
        selfSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);

    }
}