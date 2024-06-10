using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class ControllerBase
{
    private readonly List<string> _views = new List<string>();

    public virtual ViewBase ShowView(ShowViewEventArgs e)
    {
        if(!_views.Contains(e.ViewName)) return null;
        return App.Instance.ViewManager.ShowView(e);
    }

    protected void RegisterView(string viewName)
    {
        _views.Add(viewName);
    }

    protected void UnRegisterView(string viewName)
    {
        _views.Remove(viewName);
    }
}
