using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OverWorld{  

public class LightCircle : MonoBehaviour
{

        public Vector3 circle;
    void Update()
    {
            transform.Rotate(circle * Time.deltaTime);
    }
}
}