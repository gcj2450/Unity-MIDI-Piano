using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCallTestController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        RegisterControllers();
        ChangeUIEventArgs e;
        e = new ChangeUIEventArgs(EventNames.ShowStartView.ToString());
        App.Instance.EventManager.SendEvent(e);
    }
    /// <summary>
    /// 注册所有Controller,一般在程序最初主入口调用
    /// </summary>
    private void RegisterControllers()
    {
        App.Instance.ControllerManager.AddControllers(new List<ControllerBase>
            {
               new TestController()
            });
    }
    // Update is called once per frame
    void Update()
    {

    }
}
