
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//两个类使用此接口，可用此接口来调用对象，可使用as判断
public interface iEveninfo
{

}
//创建类继承自接口，以后便可以通过接口来引用此类
public class EventInfo<T> : iEveninfo
{
    //注意参数名actions,多加了一个S,此委托监听多个函数
    public Action<T> actions;

    //action，将要监听的函数
    public EventInfo(Action<T> action)
    {
        actions += action;
    }
}
//同上，不过此时的委托为无参委托
public class EventInfo : iEveninfo
{
    public Action actions;

    public EventInfo(Action action)
    {
        actions += action;
    }
}

//事件系统为单例模式，继承自BaseManager
public class EventMgr : BaseManager<EventMgr>
{
    //使用字典结构，可以方便的通过“name"来触发委托，注意此时用的是接口
    public Dictionary<string, iEveninfo> eventDic
        = new Dictionary<string, iEveninfo>();
    //添加监听，注意监听的函数为带参函数
    public void AddEventListener<T>(string eventNumber, Action<T> action)
    {
        //如果字典中已经有此eventNumber,将此方法添加至字典中
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo<T>).actions += action;//使用as来转换，此时接口转化成了EventInfo<T>
        //若没有此eventNumber,则在字典中添加事件名和事件方法
        else
            eventDic.Add(eventNumber, new EventInfo<T>(action));
    }
    //添加监听，同上不过方法无参数
    public void AddEventListener(string eventNumber, Action action)
    {
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo).actions += action;
        else
            eventDic.Add(eventNumber, new EventInfo(action));
    }
    //触发监听的事件，由于存在泛型T，及我们的所监听的函数需要一个类型为T的参数才能触发
    public void EventTrigger<T>(string eventNumber, T info)
    {
        //如果字典里存在此事件并且actions不为空
        if (eventDic.ContainsKey(eventNumber) &&
           (eventDic[eventNumber] as EventInfo<T>).actions != null
           )
            (eventDic[eventNumber] as EventInfo<T>).actions.Invoke(info);

    }
    //同上，但不需要参数
    public void EventTrigger(string eventNumber)
    {
        if (eventDic.ContainsKey(eventNumber) &&
            (eventDic[eventNumber] as EventInfo).actions != null)
            (eventDic[eventNumber] as EventInfo).actions.Invoke();
    }
    //移除监听
    public void RemoveEventListener<T>(string eventNumber, Action<T> action)
    {
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo<T>).actions -= action;
    }
    //如上，移除监听
    public void RemoveEventListener(string eventNumber, Action action)
    {
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo).actions -= action;
    }
    //清空字典
    public void Clear()
    {
        eventDic.Clear();
    }
}