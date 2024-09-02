using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSlice : MonoBehaviour
{
    [HideInInspector]public ParticleSystem ThisparticleSystem;
    public Sprite CutedSprite;
    private List<Sprite> sprites = new List<Sprite>();
    private void Start()
    {
        ThisparticleSystem = GetComponent<ParticleSystem>();
        SpriteCut.SliceSprite(CutedSprite, ref sprites, 4);
        for (int i =0; i < sprites.Count; i++)
            ThisparticleSystem.textureSheetAnimation.SetSprite(i,sprites[i]);
    }
    public void ChangeSprite(Sprite sprite)
    {

        SpriteCut.SliceSprite(sprite, ref sprites, 4);
    
        for (int i = 0; i < sprites.Count; i++)
        {
            ThisparticleSystem.textureSheetAnimation.SetSprite(i, sprites[i]);
        }
    }
}
