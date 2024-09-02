using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MonoController : BaseManager<MonoController>
{
    private Action updateEvent;
    private Action fixedUpdateEvent;
    private Action lateStartEvent;
    private float HalfTickTimer;
    public Action HalfTickAction;
    private void Start()
    {
        Invoke("LateStart", 0.1f);
    }
     private void Update()
    {
        updateEvent?.Invoke();
        HalfTick();
    }
    private void LateStart()
    {
        lateStartEvent?.Invoke();
    }
    public void LateStart(Action action)
    {
        lateStartEvent += action;
    }

    
    private void FixedUpdate()
    {
        fixedUpdateEvent?.Invoke();
    }
    public void AddUpdateListener(Action action)
    {
        updateEvent += action;
    }
    public void RemoveUpdateListener(Action action)
    {
        updateEvent -= action;
    }
    public void AddFixedUpdateListener(Action action)
    {
        fixedUpdateEvent += action;
    }
    public void RemoveFixedUpdateListener(Action action)
    {
        fixedUpdateEvent -= action;
    }
    private void HalfTick()
    {
        HalfTickTimer += Time.deltaTime;
        if (HalfTickTimer > 0.5f)
        {
            HalfTickTimer = 0 ;
            HalfTickAction?.Invoke();
        }
    }
    
    public void Invoke(float time, Action action)
    {
        StartCoroutine(InvokeTime(time, action));
    }
    public void InvokeUnScaled(float time, Action action)
    {
        StartCoroutine(InvokeTime2(time, action));

    }
        IEnumerator InvokeTime(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            
            action();
        }
    IEnumerator InvokeTime2(float time, Action action)
    {
        yield return new WaitForSecondsRealtime(time);

        action();
    }
}
