using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowStoneHander : Plants
{
    public GameObject Stone;
    private float checkCDtimer;


    protected override void Update()
    {
        base.Update();
        Stone.transform.Rotate(10 * Time.deltaTime, 0, 10 * Time.deltaTime);


        checkCDtimer += Time.deltaTime;
        if (checkCDtimer > 2)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Ray ray = new Ray(transform.position + new Vector3(i * 2, 3, j * 2), Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit, 5, 1 << 9))
                    {
                        Plants p = hit.collider.GetComponent<Plants>();
                        if (p != null)
                        {
                            IproduceSun ip = (IproduceSun)p;
                            if(ip!= null)
                            {
                                ip.ProductivityUp();
                            }
                        }
                    }
                }
            }
            checkCDtimer = 0;
        }


    }


}
public interface IproduceSun
{
    public void ProductivityUp();
}

