using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCut
{
    public static void SliceSprite(Sprite sprite, ref List<Sprite> SlicedSprites, int size)
    {
        SlicedSprites.Clear();
        Texture2D texture = sprite.texture;
      
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        float pixelsPerUnit = 16f;
        // ´´½¨Sprite
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {   
                Rect rect = new Rect(sprite.rect.x + i * sprite.rect.width/size
                    , sprite.rect.y + i * sprite.rect.height / size
                    , sprite.rect.width/size
                    , sprite.rect.height/size);
                Sprite Go = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
                Go.texture.filterMode = FilterMode.Point;
                SlicedSprites.Add(Go);
            }
        }
    }
}