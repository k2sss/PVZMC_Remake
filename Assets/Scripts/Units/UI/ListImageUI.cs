using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum InsideType
{
    full,
    half,
    empty
}

public class ListImageUI : MonoBehaviour
{
    public InsideType Itype;
    public int ID;
    public Image OutLineImage;//Õ‚Œß
    public Image InsideImage;//ƒ⁄≤ø
    public bool IsShake;
    public float ShakeSrength = 6;
    public float ShakeSpeed = 6;
    public float t;
    private Vector2 StartPos;
    private RectTransform thisRectTransform;
    public ListImageSpriteInfo Spriteinfo;

    private void Start()
    {
        thisRectTransform = GetComponent<RectTransform>();
        Invoke("GetSrartPos", 0.01f);
    }
    private void GetSrartPos()
    {
       StartPos = thisRectTransform.anchoredPosition;
    }
    private void Update()
    {
            
        if (IsShake == true)
        AnimationShake();
    }

    
    public void ChangeInside(ListImageSpriteInfo Spriteinfo)
    {
        InsideImage.sprite = Spriteinfo.insideFullSprite;
    }
    public void ChangeOutLine(ListImageSpriteInfo Spriteinfo)
    {
        OutLineImage.sprite = Spriteinfo.outlineSprite;
    }
    public void ChangeInside(Sprite sprite)
    {
        InsideImage.sprite = sprite;
    }
    public void ChangeOutLine(Sprite sprite)
    {
        OutLineImage.sprite = sprite;
    }
    public void AnimationShake()//‘Î…˘shake
    {
        t += Time.deltaTime * ShakeSpeed;
        float NoiseValue = Mathf.PerlinNoise(t, 0);
        thisRectTransform.anchoredPosition = StartPos + new Vector2(0,NoiseValue * ShakeSrength);
    }
    public void StartShake(float t)
    {
        this.t = t;
        IsShake = true;
        //Debug.Log(this.t);
    }
    public void StopShake()
    {
        thisRectTransform.anchoredPosition = StartPos;
        IsShake = false;    
    }

}
[System.Serializable]
public class ListImageSpriteInfo:ICloneable
{
    public Sprite outlineSprite;
    public Sprite outLineSprite_highlLighted;
    public Sprite insideHalfSprite;
    public Sprite insideFullSprite;
    public Sprite Empty;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
    public ListImageSpriteInfo ShallowClone()
    {
        return Clone() as ListImageSpriteInfo;
    }


}