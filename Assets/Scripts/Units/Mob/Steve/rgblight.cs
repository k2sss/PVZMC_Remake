using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rgblight : MonoBehaviour
{
    public Light targetLight; // �������еĵƹ���ק���ñ�����
    public float cycleSpeed = 1f; // �Ų��ٶȣ����Ը����������

    private float timeOffset = 0f;

    private void Start()
    {
        if (targetLight == null)
        {
           
            this.enabled = false; // ���ýű������ⷢ������
        }
    }

    private void Update()
    {
        // ����һ��0��1֮���ֵ�����ڿ�����ɫ�仯
        float t = (Mathf.Sin((Time.time + timeOffset) * cycleSpeed) + 1f) / 2f;

        // ����ʱ��t����RGB��ɫ��ֵ
        Color color = new Color(Mathf.Sin(t * 2f * Mathf.PI), Mathf.Sin(t * 2f * Mathf.PI + 2f * Mathf.PI / 3f), Mathf.Sin(t * 2f * Mathf.PI + 4f * Mathf.PI / 3f));

        // ����ɫ����Ϊ�ƹ����ɫ
        targetLight.color = color;
    }
}
