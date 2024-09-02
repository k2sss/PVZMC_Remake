using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessBar : MonoBehaviour
{
    private Sprite Bar_Container;
    private Sprite Bar;
    private Sprite Bar_ContractColor;
    private Image inside, outside;
    public float TargetFillAmout { private set; get; }
    public float  BarMoveSpeed = 1;
    private RectTransform UIGizomosTransform;
    private float Width;
    private GameObject Flag;
    private float[] values;
    private UIFlag[] flags;
    public float processValue { private set; get; }
    
    private void Start()
    {
        EventMgr.Instance.AddEventListener("PickUpChest", () => gameObject.SetActive(false));
        EventMgr.Instance.AddEventListener("GameStart", () => gameObject.SetActive(true));
        gameObject.SetActive(false);
        //获得Sprite
        Sprite[] sprites =ResourceSystem.Instance.GetSprite(SpriteType.ProcessBar);
        Bar_Container = sprites[0];
        Bar = sprites[1];
        Bar_ContractColor = sprites[2];
        //获得IMAGE
        inside = transform.GetChild(0).gameObject.GetComponent<Image>();
        outside = GetComponent<Image>();
        //更新
        if (LevelManager.Instance.IsBossFight == false)
        {
             EventMgr.Instance.AddEventListener("ChangeWave", WaveUpdate);
            MonoController.Instance.Invoke(0.1f, WaveUpdate);
        }
       

        UIGizomosTransform = transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        Width = UIGizomosTransform.localPosition.x * 2f;
        //生成FLAG
        Flag = FileLoadSystem.ResourcesLoad<GameObject>("UI/Flag");
        values = new float[LevelManager.Instance.HugeWavesArray.Length + 1];
        flags = new UIFlag[values.Length];
        for (int i = 0; i < values.Length - 1; i++)
        {
            values[i] = (float)LevelManager.Instance.HugeWavesArray[i] / LevelManager.Instance.MaxWave;
        }
        values[values.Length - 1] = 1f;

        for (int i = 0; i < LevelManager.Instance.HugeWavesArray.Length; i++)
        {
            GameObject g = Instantiate(Flag, transform);
            RectTransform gRectTrans = g.GetComponent<RectTransform>();
            gRectTrans.localPosition = new Vector3(GetRightPos(values[i]), gRectTrans.localPosition.y, gRectTrans.localPosition.z);
            flags[i] = g.GetComponent<UIFlag>();
        }
        GameObject Lg = Instantiate(Flag, transform);
        RectTransform LgRectTrans = Lg.GetComponent<RectTransform>();
        LgRectTrans.localPosition = new Vector3(GetRightPos(1), LgRectTrans.localPosition.y, LgRectTrans.localPosition.z);
        flags[flags.Length - 1] = Lg.GetComponent<UIFlag>();
        //动画

    }
    private void Update()
    {
        inside.fillAmount = Mathf.Lerp(inside.fillAmount, TargetFillAmout, BarMoveSpeed * Time.deltaTime);
        UIGizomosTransform.localPosition = new Vector3(GetRightPos(inside.fillAmount), UIGizomosTransform.localPosition.y,UIGizomosTransform.localPosition.z);
        for (int i = 0; i < flags.Length; i++)
        {
            if (inside.fillAmount >= values[i]-0.04f)
            {
                flags[i].Trigger();
            }
        }
        if (LevelManager.Instance.IsBossFight == true)
        {
            WaveUpdateWhenBossFight();
        }
    }
    public void SetTargetValue(float value)
    {
        TargetFillAmout = value;
    }
    public void OutSideHighLight(float HoldTime)
    {
        outside.sprite = Bar_ContractColor;
        MonoController.Instance.Invoke(HoldTime, () => outside.sprite = Bar_Container);
    }
    private void WaveUpdate()
    {
        processValue = (float)LevelManager.Instance.nowWave / LevelManager.Instance.MaxWave;
        float value = processValue;
        SetTargetValue(value);
        OutSideHighLight(1.2f);
    }
    private void WaveUpdateWhenBossFight()
    {
        processValue = (float)LevelManager.Instance.Boss.Health / LevelManager.Instance.Boss.MaxHealth;
        float value = 1 - processValue;
        SetTargetValue(value);
    }
    private float GetRightPos(float x)
    {
        if (x < 0 && x > 1f)
        {
            return 0;
        }
        return -Width / 2 + (1f - x) * Width;
    }

}
