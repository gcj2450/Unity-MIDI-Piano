using UnityEngine;
using System;

public class App : MonoBehaviour
{
    private static App _instance;
    /// <summary>
    /// Singleton,方便各模块访问
    /// </summary>
    public static App Instance
    {
        get
        {
            if (_instance == null)
            {
                var app = FindObjectOfType(typeof(App)) as App;
                if (app == null)
                {
                    var appObject = new GameObject("App");
                    app = appObject.AddComponent<App>();
                }
                _instance = app;
            }

            return _instance;
        }
    }

    public EventManager EventManager { get; private set; }
    public ControllerManager ControllerManager { get; private set; }
    public ViewManager ViewManager { get; private set; }

    protected virtual void Awake()
    {
        if (_instance != null) return;
        _instance = this;
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        Initialize();
    }

    protected virtual void Initialize()
    {
        EventManager = gameObject.AddComponent<EventManager>();
        ControllerManager = gameObject.AddComponent<ControllerManager>();
        ViewManager = gameObject.AddComponent<ViewManager>();
    }
}
