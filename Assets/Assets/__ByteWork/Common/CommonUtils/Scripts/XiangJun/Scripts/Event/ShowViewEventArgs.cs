
public class ShowViewEventArgs : ChangeUIEventArgs
{
    public const string EventName = "ShowView";
    public string ViewName { get; private set; }
    public bool HidePrevious { get; set; }
    public bool RemovePrevious { get; set; }
    public bool ShowWithAnimation { get; set; }
    public bool AddToUiRoot { get; set; }
    public bool DestroyAutomatically { get; set; }

    public ShowViewEventArgs(string viewName, bool hidePrevious = true, bool removePrevious = true, bool showWithAnimation = true,bool addToUiRoot=true)
        : base(EventName)
    {
        ViewName = viewName;
        HidePrevious = hidePrevious;
        RemovePrevious = removePrevious;
        ShowWithAnimation = showWithAnimation;
        AddToUiRoot = addToUiRoot;
    }
    public ShowViewEventArgs(string viewName,ChangeUIEventArgs args, bool hidePrevious = true, bool removePrevious = true, bool showWithAnimation = true, bool addToUiRoot = true)
        : base(EventName)
    {
        ViewName = viewName;

        Data = args.Data;
        Attachment = args.Attachment;
        Boolean = args.Boolean;
        SendDelegate = args.SendDelegate;
        EventCallBack = args.EventCallBack;
        EventHandler = args.EventHandler;

        HidePrevious = hidePrevious;
        RemovePrevious = removePrevious;
        ShowWithAnimation = showWithAnimation;
        AddToUiRoot = addToUiRoot;
    }
}