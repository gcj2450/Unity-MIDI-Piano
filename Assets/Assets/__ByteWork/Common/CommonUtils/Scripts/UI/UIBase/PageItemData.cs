using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
///列表页列表项数据实体
/// </summary>
public class PageItemData
{
    /// <summary>
    /// 列表项原始数据
    /// </summary>
    private object itemData;
    public object ItemData
    {
        get { return itemData; }
        set { itemData = value; }
    }
    
    private Dictionary<string, object> attachment;
    /// <summary>
    /// 用于存储界面展示用的各种数据
    /// </summary>
    public Dictionary<string, object> Attachment
    {
        get { return attachment; }
        set { attachment = value; }
    }

    private Action<object, PageItemData> onClickHandler;
    /// <summary>
    /// 列表项点击事件，参数1为被点击的Item，参数2为Item带的数据
    /// </summary>
    public Action<object, PageItemData> OnClickHandler
    {
        get { return onClickHandler; }
        set { onClickHandler = value; }
    }
    #region 构造函数，无需修改
    public PageItemData()
    {
    }
    public PageItemData(object data)
    {
        itemData = data;
    }
    public PageItemData(Dictionary<string, object> attachment)
    {
        this.attachment = attachment;
    }
    public PageItemData(object data, Action<object, PageItemData> clickHandler)
    {
        itemData = data;
        this.onClickHandler = clickHandler;
    }
    public PageItemData(Dictionary<string, object> attachment, Action<object, PageItemData> clickHandler)
    {
        this.attachment = attachment;
        this.onClickHandler = clickHandler;
    }
    public PageItemData(object data, Dictionary<string, object> attachment, Action<object, PageItemData> clickHandler)
    {
        itemData = data;
        this.attachment = attachment;
        this.onClickHandler = clickHandler;
    }
    #endregion
}
