using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ViewManager : MonoBehaviour
{
    private readonly Stack<ViewBase> _views = new Stack<ViewBase>();
    private readonly Dictionary<string, ViewBase> _viewsPool = new Dictionary<string, ViewBase>();
    public GameObject BusyFlag;
    //public GameObject NormalUiRoot { get; private set; }
    //public GameObject UiRoot { get; private set; }

    public ViewBase CurrentView
    {
        get { return _views.Count != 0 ? _views.Peek() : null; }
    }

    //private static ViewManager _instance;
    ///// <summary>
    ///// Singleton,方便各模块访问
    ///// </summary>
    //public static ViewManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            var app = FindObjectOfType(typeof(ViewManager)) as ViewManager;
    //            if (app == null)
    //            {
    //                var appObject = new GameObject("ViewManager");
    //                app = appObject.AddComponent<ViewManager>();
    //            }
    //            _instance = app;
    //        }

    //        return _instance;
    //    }
    //}

    void Awake()
    {
    }

    RectTransform GetUIRoot()
    {
        return GameObject.Find("Canvas/Root").GetComponent<RectTransform>();
    }


    public ViewBase ShowView(ShowViewEventArgs args)
    {
        //Debug.Log("ViewManager ShowView");
        var currentView = CurrentView;
        if (currentView != null && currentView.Name == args.ViewName)
        {
            currentView.Show(currentView.Args);
            return CurrentView;
        }
        //handle previous view
        if (_views.Count > 0)
        {
            var previous = _views.Peek();
            if (args.HidePrevious)
            {
                previous.Hide();
            }
            if (args.RemovePrevious)
            {
                _views.Pop();
                BackToPool(previous);
            }
        }
        //create view or get from stack
        var view = _views.FirstOrDefault(p => p.Name == args.ViewName);
        if (view == null)
        {
            view = CreateView(args.ViewName);
            view.Name = args.ViewName;
            if (args.AddToUiRoot)
            {
                view.GetComponent<RectTransform>().SetParent(GetUIRoot(), false);
                view.transform.SetAsLastSibling();
            }
            _views.Push(view);
        }
        else
        {
            while (_views.Count > 0)
            {
                if (_views.Peek().Name == args.ViewName)
                {
                    view = _views.Peek();
                    break;
                }
                var temp = _views.Pop();
                temp.gameObject.SetActive(false);
                BackToPool(temp);
            }
        }
        //show view
        if (view != null)
        {
            view.Show(args);
        }
        return view;
    }

    public void GoBack()
    {
        if(_views.Count<=1) return;
        var view = _views.Peek();
        _views.Pop();
        view.Hide();
        BackToPool(view);
        var pre = _views.Peek();
        _views.Peek().Show(pre.Args);
    }
	public ViewBase GetViewByname(string viewname)
	{
		if (CurrentView.Name == viewname)
		{
			return CurrentView;
		}
		else if (_viewsPool.ContainsKey (viewname)) 
		{
			return _viewsPool [viewname];
		}
		return null;
	}
    private void BackToPool(ViewBase view)
    {
        if (view.Args != null && view.Args.DestroyAutomatically)
        {
            return;
        }
        if (!_viewsPool.ContainsKey(view.Name))
        {
            _viewsPool.Add(view.Name, view);
        }
    }

    public ViewBase CreateView(string viewName)
    {
        Debug.Log("CreateView" + viewName);
        if (string.IsNullOrEmpty(viewName)) return null;
        if (_viewsPool.ContainsKey(viewName))
        {
            return _viewsPool[viewName];
        }
        var prefab = Resources.Load("Views/" + viewName) as GameObject;
        if (prefab == null) return null;
        var viewObject = Instantiate(prefab);
        viewObject.name = viewName;
        return viewObject.GetComponent<ViewBase>();
    }


    /// <summary>
    /// 判断忙碌标记是否显示
    /// </summary>
    /// <returns></returns>
    protected bool CheckBusyFlag()
    {
        return BusyFlag != null && BusyFlag.activeSelf;
    }

    /// <summary>
    /// 显示或隐藏忙碌标记
    /// </summary>
    /// <param name="show"></param>
    public void ShowBusyFlag(bool show)
    {
        BusyFlag.SetActive(show);
    }

}
