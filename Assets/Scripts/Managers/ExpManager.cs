using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExpManager : BaseManager<ExpManager>
{
    public Image ValueImage;
    public Text text;
    private void Start()
    {
        PlayerExp.Instance.OnExpAdd += UpdateImage;
        
    }
    public void UpdateImage()
    {
        ValueImage.fillAmount = (float)PlayerExp.Instance.Exp / PlayerExp.Instance.MaxExp;
        if (PlayerExp.Instance.Level != 0)
            text.text = PlayerExp.Instance.Level.ToString();
        else
            text.text = "";
    }
}
