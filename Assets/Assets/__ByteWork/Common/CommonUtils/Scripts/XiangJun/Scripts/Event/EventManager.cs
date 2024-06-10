using System;
using System.Collections.Generic;
using UnityEngine;
//EventManager.RegisterHandler(ShowViewEventArgs.EventName, ShowViewHandler);
//EventManager.SendEvent(new CustomEventArgs(EventNames.StartSolar.ToString()));
public class EventManager : MonoBehaviour
{
    public delegate void EventHandler(ChangeUIEventArgs e);
    private readonly Queue<ChangeUIEventArgs> _events = new Queue<ChangeUIEventArgs>();
    private readonly Dictionary<string, EventHandler> _handlerDictionary = new Dictionary<string, EventHandler>();

    void Update()
    {
        while (true)
        {
            lock (_events)
            {
                if (_events.Count == 0) break;
            }

            ChangeUIEventArgs e;
            lock (_events)
            {
                e = _events.Dequeue();
            }
            HandleEvent(e);
        }
    }

    /// <summary>
    /// 注册监听事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handler"></param>
    public void RegisterHandler(string eventName, EventHandler handler)
    {
        if (string.IsNullOrEmpty(eventName) || handler == null) return;

        if (!_handlerDictionary.ContainsKey(eventName))
        {
            _handlerDictionary.Add(eventName, null);
        }

        _handlerDictionary[eventName] = _handlerDictionary[eventName] + handler;
    }

    /// <summary>
    /// 取消监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handler"></param>
    public void UnRegisterHandler(string eventName, EventHandler handler)
    {
        if (string.IsNullOrEmpty(eventName) || handler == null || !_handlerDictionary.ContainsKey(eventName) || _handlerDictionary[eventName] == null) return;

        _handlerDictionary[eventName] = _handlerDictionary[eventName] - handler;
        if (_handlerDictionary[eventName] == null)
        {
            _handlerDictionary.Remove(eventName);
        }
    }
    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="e"></param>
    public void SendEvent(ChangeUIEventArgs e)
    {
        if (e == null) return;
        if (!_handlerDictionary.ContainsKey(e.Name))
        {
            return;
        }
        if (e.Boolean)
        {
            HandleEvent(e);
        }
        else
        {
            lock (_events)
            {
                _events.Enqueue(e);
            }
        }
    }

    private void HandleEvent(ChangeUIEventArgs e)
    {
        if (e == null) return;
        if (!_handlerDictionary.ContainsKey(e.Name) || _handlerDictionary[e.Name] == null)
        {
            return;
        }

        _handlerDictionary[e.Name](e);
    }

    /** the clean method
         *
         * you must invoke this method at last
         */
    public void Shutdown()
    {
        lock (_events)
        {
            _events.Clear();
        }
        _handlerDictionary.Clear();
    }
}