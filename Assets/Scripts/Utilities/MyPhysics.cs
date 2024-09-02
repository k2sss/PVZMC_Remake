using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPhysics : MonoBehaviour
{
    public static T RayCheckToFindTarget<T>(Vector3[] trans, Vector3 dir, float length, int layer, string tag)//多条射线检测
    {


        Ray[] ray = new Ray[trans.Length];
        for (int i = 0; i < trans.Length; i++)
        {
            ray[i] = new Ray(trans[i], dir);
        }
        RaycastHit hit;
#if UNITY_EDITOR
        for (int i = 0; i < ray.Length; i++)
        {
            Debug.DrawLine(trans[i], trans[i] + dir.normalized * length);
        }
#endif
        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, length, 1 << layer) && hit.collider.CompareTag(tag))
            {
                return hit.collider.GetComponent<T>();
            }
        }
        return default(T);
    }
    public static T RayCheckToFindTarget<T>(Vector3[] trans, Vector3 dir, float length, int layer)//多条射线检测
    {


        Ray[] ray = new Ray[trans.Length];
        for (int i = 0; i < trans.Length; i++)
        {
            ray[i] = new Ray(trans[i], dir);
        }
        RaycastHit hit;
#if UNITY_EDITOR
        for (int i = 0; i < ray.Length; i++)
        {
            Debug.DrawLine(trans[i], trans[i] + dir.normalized * length);
        }
#endif
        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, length, 1 << layer))
            {
                return hit.collider.GetComponent<T>();
            }
        }
        return default(T);
    }
    public static T BoxRayCheck<T>(Vector3 origin, float extendsLength, Vector3 dir, float length, int layer)
    {
        RaycastHit hit;
#if UNITY_EDITOR
        Debug.DrawLine(origin, origin + dir * length);
        Debug.DrawLine(origin, origin + dir * extendsLength/2,Color.red);
#endif
        if (Physics.BoxCast(origin - dir.normalized*extendsLength/2, new Vector3(extendsLength, extendsLength, extendsLength) / 2, dir, out hit, Quaternion.identity, length, 1 << layer))
        {
            return hit.collider.GetComponent<T>();
        }
        return default(T);
    }
    public static bool BoxRayCheck(Vector3 origin, float extendsLength, Vector3 dir, float length, int layer)
    {
        RaycastHit hit;
#if UNITY_EDITOR
        Debug.DrawLine(origin, origin + dir * length);
        Debug.DrawLine(origin, origin + dir * extendsLength / 2, Color.red);
#endif
        if (Physics.BoxCast(origin - dir.normalized * extendsLength / 2, new Vector3(extendsLength, extendsLength, extendsLength) / 2, dir, out hit, Quaternion.identity, length, 1 << layer))
        {
            return true;
        }
        return false;
    }
    public static int BoxRayCheckNum<T>(Vector3 origin, float extendsLength, Vector3 dir, float length, int layer)
    {
#if UNITY_EDITOR
        Debug.DrawLine(origin, origin + dir * length);
        Debug.DrawLine(origin, origin + dir * extendsLength / 2, Color.blue);
#endif
        return Physics.BoxCastAll(origin - dir.normalized * extendsLength / 2, new Vector3(extendsLength, extendsLength, extendsLength) / 2, dir, Quaternion.identity, length, 1 << layer).Length ;
    }
    public static T BoxRayCheck<T>(Vector3 origin, float extendsLength,float height, Vector3 dir, float length, int layer)
    {
        RaycastHit hit;
#if UNITY_EDITOR
        Debug.DrawLine(origin, origin + dir * length);
        Debug.DrawLine(origin, origin + dir * extendsLength / 2, Color.red);
#endif
        if (Physics.BoxCast(origin - dir.normalized * extendsLength / 2, new Vector3(extendsLength, height, extendsLength) / 2, dir, out hit, Quaternion.identity, length, 1 << layer))
        {
            return hit.collider.GetComponent<T>();
        }
        return default(T);
    }

    public static bool CheckBox(Vector3 origin, float extendsLength, int layer)
    {
        if (Physics.CheckBox(origin, new Vector3(extendsLength, extendsLength, extendsLength) / 2, Quaternion.identity, 1 << layer))
        {
            return true;
        }

        return false;
    }
}
