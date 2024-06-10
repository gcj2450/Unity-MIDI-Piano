using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 仿NGUI的事件监听系统，使用方法
/// EventTriggerListener.Get(CubeObj).onClick += CubeClick;
/// </summary>
public class EventTriggerListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void VoidDelegateWithData(GameObject go,PointerEventData ped);
    public delegate void VoidDelegateWithBaseData(GameObject go, BaseEventData bed);
    public delegate void VoidDelegateWithAxisData(GameObject go, AxisEventData bed);
    public VoidDelegate onEnter;
    public VoidDelegateWithData onEnterWithData;

    public VoidDelegate onExit;
    public VoidDelegateWithData onExitWithData;

    public VoidDelegate onDown;
    public VoidDelegateWithData onDownWithData;

    public VoidDelegate onUp;
    public VoidDelegateWithData onUpWithData;

    public VoidDelegate onClick;
    public VoidDelegateWithData onClickWithData;

    public VoidDelegate onDrag;
    public VoidDelegateWithData onDragWithData;

    public VoidDelegate onDrop;
    public VoidDelegateWithData onDropWithData;

    public VoidDelegate onScroll;
    public VoidDelegateWithData onScrollWithData;

    public VoidDelegate onUpdateSelect;
    public VoidDelegateWithBaseData onUpdateSelectWithData;

    public VoidDelegate onSelect;
    public VoidDelegateWithBaseData onSelectWithData;

    public VoidDelegate onDeSelect;
    public VoidDelegateWithBaseData onDeSelectWithData;

    public VoidDelegate onMove;
    public VoidDelegateWithAxisData onMoveWithData;

    public VoidDelegate onInitializePotentialDrag;
    public VoidDelegateWithData onInitializePotentialDragWithData;

    public VoidDelegate onBeginDrag;
    public VoidDelegateWithData onBeginDragWithData;

    public VoidDelegate onEndDrag;
    public VoidDelegateWithData onEndDragWithData;

    public VoidDelegate onSubmit;
    public VoidDelegateWithBaseData onSubmitWithData;

    public VoidDelegate onCancel;
    public VoidDelegateWithBaseData onCancelWithData;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
        if (onEnterWithData != null) onEnterWithData(gameObject, eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
        if (onExitWithData != null) onExitWithData(gameObject,eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
        if (onDownWithData != null) onDownWithData(gameObject,eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
        if (onUpWithData != null) onUpWithData(gameObject,eventData);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
        if (onClickWithData != null) onClickWithData(gameObject,eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject);
        if (onDragWithData != null) onDragWithData(gameObject,eventData);
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null) onDrop(gameObject);
        if (onDropWithData != null) onDropWithData(gameObject,eventData);
    }
    public override void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null) onScroll(gameObject);
        if (onScrollWithData != null) onScrollWithData(gameObject,eventData);
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        if (onDeSelect != null) onDeSelect(gameObject);
        if (onDeSelectWithData != null) onDeSelectWithData(gameObject, eventData);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(gameObject);
        if (onBeginDragWithData != null) onBeginDragWithData(gameObject,eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(gameObject);
        if (onEndDragWithData != null) onEndDragWithData(gameObject,eventData);
    }
    public override void OnMove(AxisEventData eventData)
    {
        if (onMove != null) onMove(gameObject);
        if (onMoveWithData != null) onMoveWithData(gameObject,eventData);
    }
    public override void OnCancel(BaseEventData eventData)
    {
        if (onCancel != null) onCancel(gameObject);
        if (onCancelWithData != null) onCancelWithData(gameObject,eventData);
    }
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (onInitializePotentialDrag != null) onInitializePotentialDrag(gameObject);
        if (onInitializePotentialDragWithData != null) onInitializePotentialDragWithData(gameObject,eventData);
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(gameObject);
        if (onSubmitWithData != null) onSubmitWithData(gameObject,eventData);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
        if (onSelectWithData != null) onSelectWithData(gameObject,eventData);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
        if (onUpdateSelectWithData != null) onUpdateSelectWithData(gameObject,eventData);
    }
}