using System;
using System.Collections;
using System.Collections.Generic;
using SimpleTween;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 栏目列表项预制体，控制列表项显示和交互动效
/// 点击事件响应: HviEPGViewPresenter.OnColumnItemClick
/// </summary>
public class ColumnItem : PageItem
{
    public Image BgImg = null;
    /// <summary>
    /// 栏目名称
    /// </summary>
    public Text ColumnNameText = null;

    public override void SetData(PageItemData data)
    {
        base.SetData(data);
        OnClickHandler = data.OnClickHandler;
        ColumnNameText.text = (string)data.Attachment["columnName"];
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    protected override void OnClick()
    {
        //注意点击事件中的选中和取消选中的逻辑处理，
        //在HviEPGViewPresenter层实现
        base.OnClick();
    }

    /// <summary>光标进入事件
    /// </summary>
    protected override void OnEnter()
    {
        base.OnEnter();
        EnterState();
    }

    /// <summary>光标离开事件
    /// </summary>
    protected override void OnExit()
    {
        base.OnExit();
        if (!isSelected)
            NormalState();
    }

    /// <summary>设置选中状态
    /// 设置按钮为选中状态，
    /// true为发送点击事件，相当于点击按钮
    /// </summary>
    /// <param name="_fireEvnt">是否发送click事件</param>
    public void SetSelected(bool fireEvnt)
    {
        if (fireEvnt)
        {
            if (OnClickHandler != null)
                OnClickHandler(this, Data);
        }
        isSelected = true;
        SelState();
    }

    /// <summary>
    /// 取消按钮选中状态
    /// </summary>
    public void DeSelect()
    {
        isSelected = false;
        NormalState();
    }

    /// <summary>
    /// 设置选中状态
    /// </summary>
    private void SelState()
    {
		SimpleTweener.AddTween(() => transform.localScale, x => transform.localScale = x, Vector3.one, AnimationTime);
        BgImg.color = new Color(1, 1, 1, 0.3f);
        ColumnNameText.color = new Color(1, 1, 1, 1f);
        Vector3 targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
		SimpleTweener.AddTween(() => transform.localPosition, x => transform.localPosition = x, targetPos, AnimationTime);
    }

    /// <summary>
    /// 设置进入状态
    /// </summary>
    private void EnterState()
    {
        if (isSelected)
            return;
		SimpleTweener.AddTween(() => transform.localScale, x => transform.localScale = x, Vector3.one* AnimationToScale, AnimationTime);
        BgImg.color = new Color(1, 1, 1, 0.3f);
        ColumnNameText.color = new Color(1, 1, 1, 1f);
        Vector3 targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, AnimationToOffset);
		SimpleTweener.AddTween(() => transform.localPosition, x => transform.localPosition = x, targetPos, AnimationTime);
    }

    /// <summary>
    /// 设置正常状态
    /// </summary>
    private void NormalState()
    {
		SimpleTweener.AddTween(() => transform.localScale, x => transform.localScale = x, Vector3.one, AnimationTime);
        BgImg.color = new Color(1f, 1f, 1f, 0.2f);
        ColumnNameText.color = new Color(1f, 1f, 1f, 0.2f);
        Vector3 targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
		SimpleTweener.AddTween(() => transform.localPosition, x => transform.localPosition = x, targetPos, AnimationTime);
    }
}
