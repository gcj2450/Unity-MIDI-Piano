using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : ControllerBase
{
    public TestController()
    {
        RegisterView(ViewNames.StartView.ToString());
        RegisterView(ViewNames.EndView.ToString());
        App.Instance.EventManager.RegisterHandler(EventNames.ShowStartView.ToString(), OnShow);
    }

    public override ViewBase ShowView(ShowViewEventArgs e)
    {
        var view = base.ShowView(e);
        if (e.ViewName == ViewNames.StartView.ToString())
        {
            Debug.Log("This will show start View");
            ShowViewEventArgs showArgs = new ShowViewEventArgs("TestView");
            showArgs.Data = "TestCtt gcj called this  ";
            App.Instance.ViewManager.ShowView(showArgs);
        }
        if (e.ViewName == ViewNames.EndView.ToString())
        {
            Debug.Log("ViewNames.StartView");
        }
        return view;
    }

    private void OnShow(ChangeUIEventArgs e)
    {
        Debug.Log("TestController OnShow");
        ShowView(new ShowViewEventArgs(ViewNames.StartView.ToString()));
    }
}
