using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyClass.Extensions.Vector3Extension
{
    public static class VectorExtensions
    {
        public static Vector3 Clamp(this Vector3 origin,Vector3 leftdown,Vector3 rightup)
        {
            float x = Mathf.Clamp(origin.x,leftdown.x,rightup.x);
            float y = Mathf.Clamp(origin.y,leftdown.y,rightup.y);
            float z = Mathf.Clamp(origin.z,leftdown.z,rightup.z);
            return new Vector3(x,y,z);
        }
        public static Vector3 ClampIgnoreYAxis(this Vector3 origin, Vector3 leftdown, Vector3 rightup)
        {
            float x = Mathf.Clamp(origin.x, leftdown.x, rightup.x);
            float z = Mathf.Clamp(origin.z, leftdown.z, rightup.z);
            return new Vector3(x, origin.y, z);
        }
    }

}
