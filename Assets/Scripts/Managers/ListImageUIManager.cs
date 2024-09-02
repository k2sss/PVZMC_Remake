using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ListImageUIManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public RectTransform eparent;
    private void Awake()
    {
        instance = this as T;
    }
    public List<ListImageUIGroup> groups = new List<ListImageUIGroup>();
    public void Init(GameObject prefab)
    {
        ListImageUI u = prefab.GetComponent<ListImageUI>();
        ListImageUIGroup g = new ListImageUIGroup(u.ID, u.Spriteinfo);
        groups.Add(g);
    }

    public void InfoUpdate(int value, GameObject prefab)
    {
        ListImageUI u = prefab.GetComponent<ListImageUI>();
        foreach (ListImageUIGroup g in groups)
        {
            if (g.ID == u.ID)
            {
                g.ValueUpdate(value);
                //Debug.Log("Update");
                break;
            }
        }
    }
    public void Add(GameObject prefab)
    {
        ListImageUI u = prefab.GetComponent<ListImageUI>();
        foreach (ListImageUIGroup g in groups)
        {
            if (g.ID == u.ID)
            {
                GameObject p = Instantiate(prefab, transform);
                g.AddaNewOBJ(p.GetComponent<ListImageUI>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(eparent);
                break;
            }
        }
    }
    public void DeleteAll()
    {


        foreach (ListImageUIGroup g in groups)
        {
            for (int i = 0; i < g.uiList.Count; i++)
                Destroy(g.uiList[i].gameObject);
            g.RemoveAll();
        }

    }

    public void MakeAGroupShake(GameObject prefab)
    {
        ListImageUI u = prefab.GetComponent<ListImageUI>();
        foreach (ListImageUIGroup g in groups)
        {
            if (g.ID == u.ID)
            {
                g.ShakeAll();
                break;
            }
        }
    }

    public void MakeAGroupStopShake(GameObject prefab)
    {
        ListImageUI u = prefab.GetComponent<ListImageUI>();
        foreach (ListImageUIGroup g in groups)
        {
            if (g.ID == u.ID)
            {
                g.StopShake();
                break;
            }
        }
    }
    public void MakeAllHighLightOnce(float HighLightTime)
    {
        foreach (ListImageUIGroup g in groups)
        {
            g.OutLineHighLight();
            StartCoroutine(ActionCounter(g.OutLineDefault, HighLightTime));
        }
    }
    public void MakeAGroupChangeSprite(GameObject prefab,ListImageSpriteInfo spriteInfo)
    {
        int ID = prefab.GetComponent<ListImageUI>().ID;
        foreach (ListImageUIGroup g in groups)
        {
            if (g.ID == ID)
            {
                g.ChangeSpriteInfo(spriteInfo);
            }
        }
    }
    public void MakeAGroupSpriteDeafault(GameObject prefab)
    {
        int ID = prefab.GetComponent<ListImageUI>().ID;
        foreach (ListImageUIGroup g in groups)
        {
            if (g.ID == ID)
            {
                g.ReturnDefaultSpriteInfo();
            }
        }
    }
    public int MyCompute(int a)
    {
        if (a % 2 == 0)
            return a / 2;
        else
            return a / 2 + 1;
    }

    public IEnumerator ActionCounter(Action action, float HighLightRecoverTime)
    {
        yield return new WaitForSeconds(HighLightRecoverTime);
        action();
    }
}
public class ListImageUIGroup
{
    public int ID;
    public List<ListImageUI> uiList = new List<ListImageUI>();
    public ListImageSpriteInfo Spriteinfo;
    public ListImageSpriteInfo TempleSpriteInfo;

    public ListImageUIGroup(int ID, ListImageSpriteInfo spriteInfo)
    {
        this.ID = ID;
        Spriteinfo = spriteInfo.ShallowClone();
        TempleSpriteInfo = spriteInfo.ShallowClone();
    }

    public bool IsShake()
    {
        if (uiList[0].IsShake == true)
            return true;
        else
            return false;
    }
    public void ShakeAll()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].StartShake((i % 2) / 2f * uiList[i].ShakeSpeed);
        }
    }
    public void StopShake()
    {

        foreach (ListImageUI u in uiList)
        {
            u.StopShake();
        }
    }
    public void ValueUpdate(int value)//更新显示状态
    {
        if (value % 2 == 0)
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                if (i < value / 2)
                    uiList[i].Itype = InsideType.full;
                else
                    uiList[i].Itype = InsideType.empty;
            }
        }
        else
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                if (i < value / 2)
                    uiList[i].Itype = InsideType.full;
                else if (i == value / 2)
                    uiList[i].Itype = InsideType.half;
                else if (i > value / 2)
                    uiList[i].Itype = InsideType.empty;
            }
        }
        SpriteUpdate();
    }
    public void SpriteUpdate()//贴图更新
    {
        foreach (ListImageUI u in uiList)
        {
            if (u.Itype == InsideType.full)
                u.ChangeInside(TempleSpriteInfo.insideFullSprite);
            else if (u.Itype == InsideType.empty)
                u.ChangeInside(TempleSpriteInfo.Empty);
            else if (u.Itype == InsideType.half)
                u.ChangeInside(TempleSpriteInfo.insideHalfSprite);

        }
    }
    public void RemoveAll()
    {
        uiList.Clear();

    }
    public void AddaNewOBJ(ListImageUI a)//添加一颗新的心
    {
        a.ChangeOutLine(TempleSpriteInfo);
        uiList.Add(a);
        SpriteUpdate();
    }
    public void ChangeSpriteInfo(ListImageSpriteInfo info)
    {
        TempleSpriteInfo = info.ShallowClone();
        SpriteUpdate();
    }
    public void ReturnDefaultSpriteInfo()
    {
        TempleSpriteInfo = Spriteinfo.ShallowClone();
        SpriteUpdate();
    }//回到默认图像状态
    public void OutLineHighLight()//边框高亮
    {
        foreach (ListImageUI a in uiList)
        {
            a.ChangeOutLine(Spriteinfo.outLineSprite_highlLighted);
        }

    }
    public void OutLineDefault()//边框变Default
    {
        foreach (ListImageUI a in uiList)
        {
            a.ChangeOutLine(Spriteinfo.outlineSprite);
        }
    }


}