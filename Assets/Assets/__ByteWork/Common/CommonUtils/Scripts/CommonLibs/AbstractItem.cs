using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Item抽象类
/// </summary>
public abstract class AbstractItem: MonoBehaviour
{
    private bool isInitComplete = false;
    protected object data;
    public virtual object Data
    {
        get
        {
            return this.data;
        }
        set
        {
            data = value;
        }
    }
    protected bool IsInitComplete
    {
        get
        {
            return this.isInitComplete;
        }
    }
    /// <summary>
    /// 初始化并更新Item内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void InitItem(Object sender, ChangeUIEventArgs args)
    {
        if (!isInitComplete)
        {
            isInitComplete = true;
            Init(sender, args);
        }
        UpdateItem(sender, args);
    }
    /// <summary>
    /// 初始化Item内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected abstract void Init(object sender, ChangeUIEventArgs args);
    /// <summary>
    /// 更新Item内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected abstract void UpdateItem(object sender, ChangeUIEventArgs args);
}
