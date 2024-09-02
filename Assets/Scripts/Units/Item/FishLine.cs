using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLine : MonoBehaviour
{
	LineRenderer MyL;
	public Transform Starttransform;
	public Transform endtransform;
	public Transform fishertransform;
	public Sprite[] sprites;
	public bool IsAble{get;set;}

	private Vector3 A1;
	private Vector3 A2;
	private Vector3 A3;

	private void Start()
    {
        MyL= GetComponent<LineRenderer>();
		MyL.enabled = false;
    }
    private void Update()
    {
		if (endtransform != null)
        {
		 A1 = Starttransform.position;
		 A2 = (endtransform.position - Starttransform.position) / 2 + Starttransform.position - new Vector3(0,1,0);
		 A3 = endtransform.position;
		 DrawCurve(A1, A2, A3);
        }
		
    }
    void DrawCurve(Vector3 point1, Vector3 point2, Vector3 point3)
	{
		int vertexCount = 10;//采样点数量
		List<Vector3> pointList = new List<Vector3>();

		for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
		{
			Vector3 tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
			Vector3 tangentLineVectex2 = Vector3.Lerp(point2, point3, ratio);
			Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVectex2, ratio);
			pointList.Add(bezierPoint);
		}
		MyL.positionCount = pointList.Count;
		MyL.SetPositions(pointList.ToArray());
	}
    public void Enable()
    {
		MonoController.Instance.Invoke(0.05f, () => MyL.enabled = true);
		IsAble = true;
		ChangeSprite(sprites[1]);
	}
    public void Disable()
    {
		MyL.enabled = false;
		IsAble = false;
		ChangeSprite(sprites[0]);

	}
	private void ChangeSprite(Sprite sprite)
    {
		for(int i =0;i<fishertransform.childCount;i++)
        {
			SpriteRenderer r = fishertransform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
			if (r != null)
            {
				r.sprite = sprite;
            }
			
        }

    }
}
