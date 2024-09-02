using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rgblight : MonoBehaviour
{
    public Light targetLight; // 将场景中的灯光拖拽到该变量中
    public float cycleSpeed = 1f; // 炫彩速度，可以根据需求调整

    private float timeOffset = 0f;

    private void Start()
    {
        if (targetLight == null)
        {
           
            this.enabled = false; // 禁用脚本，以免发生错误
        }
    }

    private void Update()
    {
        // 计算一个0到1之间的值，用于控制颜色变化
        float t = (Mathf.Sin((Time.time + timeOffset) * cycleSpeed) + 1f) / 2f;

        // 根据时间t计算RGB颜色的值
        Color color = new Color(Mathf.Sin(t * 2f * Mathf.PI), Mathf.Sin(t * 2f * Mathf.PI + 2f * Mathf.PI / 3f), Mathf.Sin(t * 2f * Mathf.PI + 4f * Mathf.PI / 3f));

        // 将颜色设置为灯光的颜色
        targetLight.color = color;
    }
}
