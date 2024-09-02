using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CardOnce : Card
{
    protected override bool CheckCanPlant()
    {
        return true;
    }
    public override void OnPlant()
    {
        base.OnPlant();
        Destroy(gameObject);
    }
}
