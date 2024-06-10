using System;
using UnityEngine;
using System.Collections.Generic;

public class ControllerManager : MonoBehaviour
{
    private readonly List<ControllerBase> _controllers = new List<ControllerBase>();

    //private static ControllerManager _instance;
    ///// <summary>
    ///// Singleton,方便各模块访问
    ///// </summary>
    //public static ControllerManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            var app = FindObjectOfType(typeof(ControllerManager)) as ControllerManager;
    //            if (app == null)
    //            {
    //                var appObject = new GameObject("ControllerManager");
    //                app = appObject.AddComponent<ControllerManager>();
    //            }
    //            _instance = app;
    //        }

    //        return _instance;
    //    }
    //}

    void Awake()
    {
        App.Instance.EventManager.RegisterHandler(ShowViewEventArgs.EventName, ShowViewHandler);
    }
    private void ShowViewHandler(ChangeUIEventArgs e)
    {
        var showViewEvent = (ShowViewEventArgs)e;
        for (int i = 0; i < _controllers.Count ; i++)
        {
            _controllers[i].ShowView(showViewEvent);
        }
    }

    public void AddControllers(List<ControllerBase> controllers)
    {
        if (controllers != null && controllers.Count > 0)
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                _controllers.Add(controllers[i]);
            }
        }
    }
}
