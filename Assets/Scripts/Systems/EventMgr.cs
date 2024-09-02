
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//������ʹ�ô˽ӿڣ����ô˽ӿ������ö��󣬿�ʹ��as�ж�
public interface iEveninfo
{

}
//������̳��Խӿڣ��Ժ�����ͨ���ӿ������ô���
public class EventInfo<T> : iEveninfo
{
    //ע�������actions,�����һ��S,��ί�м����������
    public Action<T> actions;

    //action����Ҫ�����ĺ���
    public EventInfo(Action<T> action)
    {
        actions += action;
    }
}
//ͬ�ϣ�������ʱ��ί��Ϊ�޲�ί��
public class EventInfo : iEveninfo
{
    public Action actions;

    public EventInfo(Action action)
    {
        actions += action;
    }
}

//�¼�ϵͳΪ����ģʽ���̳���BaseManager
public class EventMgr : BaseManager<EventMgr>
{
    //ʹ���ֵ�ṹ�����Է����ͨ����name"������ί�У�ע���ʱ�õ��ǽӿ�
    public Dictionary<string, iEveninfo> eventDic
        = new Dictionary<string, iEveninfo>();
    //��Ӽ�����ע������ĺ���Ϊ���κ���
    public void AddEventListener<T>(string eventNumber, Action<T> action)
    {
        //����ֵ����Ѿ��д�eventNumber,���˷���������ֵ���
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo<T>).actions += action;//ʹ��as��ת������ʱ�ӿ�ת������EventInfo<T>
        //��û�д�eventNumber,�����ֵ�������¼������¼�����
        else
            eventDic.Add(eventNumber, new EventInfo<T>(action));
    }
    //��Ӽ�����ͬ�ϲ��������޲���
    public void AddEventListener(string eventNumber, Action action)
    {
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo).actions += action;
        else
            eventDic.Add(eventNumber, new EventInfo(action));
    }
    //�����������¼������ڴ��ڷ���T�������ǵ��������ĺ�����Ҫһ������ΪT�Ĳ������ܴ���
    public void EventTrigger<T>(string eventNumber, T info)
    {
        //����ֵ�����ڴ��¼�����actions��Ϊ��
        if (eventDic.ContainsKey(eventNumber) &&
           (eventDic[eventNumber] as EventInfo<T>).actions != null
           )
            (eventDic[eventNumber] as EventInfo<T>).actions.Invoke(info);

    }
    //ͬ�ϣ�������Ҫ����
    public void EventTrigger(string eventNumber)
    {
        if (eventDic.ContainsKey(eventNumber) &&
            (eventDic[eventNumber] as EventInfo).actions != null)
            (eventDic[eventNumber] as EventInfo).actions.Invoke();
    }
    //�Ƴ�����
    public void RemoveEventListener<T>(string eventNumber, Action<T> action)
    {
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo<T>).actions -= action;
    }
    //���ϣ��Ƴ�����
    public void RemoveEventListener(string eventNumber, Action action)
    {
        if (eventDic.ContainsKey(eventNumber))
            (eventDic[eventNumber] as EventInfo).actions -= action;
    }
    //����ֵ�
    public void Clear()
    {
        eventDic.Clear();
    }
}