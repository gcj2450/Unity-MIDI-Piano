using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XJTestView : ViewBase
{
    public override void Show(ShowViewEventArgs args)
    {
        base.Show(args);
        Debug.Log((string)args.Data);
    }
}
