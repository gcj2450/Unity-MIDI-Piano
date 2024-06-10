using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Lean;

public class TreeViewItem : PageItem
{
    public Action<PointerEventData> OnBeginDragEvt;
    public Action<PointerEventData> OnDragEvt;
    public Action<PointerEventData> OnEndDragEvt;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetData(PageItemData data)
    {
        //PageItemData pid=(PageItemData)data;
        base.SetData(data);
        
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragEvt != null)
        {
            OnBeginDragEvt(eventData);
        }
    }
    /// <summary>
    /// 拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDrag(PointerEventData eventData)
    {
        if (OnDragEvt != null)
        {
            OnDragEvt(eventData);
        }
    }
    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragEvt != null)
        {
            OnEndDragEvt(eventData);
        }
    }
}
