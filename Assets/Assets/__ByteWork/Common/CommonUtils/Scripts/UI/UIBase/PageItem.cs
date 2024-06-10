using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// 列表页列表项基类
/// </summary>
public class PageItem : ComponentBase
{
    private bool playScaleAnimation;
    //private float scale = 1;

    public RawImage ItemTexture;

    //点击是否播放音效
    [HideInInspector]
	public bool ClickPlayAudio=true;
    /// <summary>
    /// 识别item标记
    /// </summary>
    [HideInInspector]
    public int Id;
    /// <summary>
    /// 在列表控件中全局索引
    /// </summary>
    public int GlobalIndex { get; private set; }

    /// <summary>
    /// 业务数据
    /// </summary>
    public PageItemData Data { get; set; }

    private Action<object,PageItemData> onClickHandler = null;
    public Action<object, PageItemData> OnClickHandler
    {
        get { return onClickHandler; }
        set { onClickHandler = value; }
    }

    /// <summary>
    ///  获取焦点动画时间
    /// </summary>
    protected float AnimationTime = 0.2f;

    /// <summary>
    /// 获取焦点动画Z轴偏移
    /// </summary>
    protected float AnimationToOffset = -20f;

    /// <summary>
    /// 获取焦点Scale放大比例
    /// </summary>
    protected float AnimationToScale = 1.1f;

    /// <summary>
    /// 设置item初始状态（含全局索引并设置可以反复点击)
    /// </summary>
    /// <param name="index"></param>
    public virtual void Initialize(int index)
    {
        GlobalIndex = index;
    }
    /// <summary>用于标记该按钮是否为选中状态
    /// </summary>
    [HideInInspector]
    public bool isSelected = false;
    /// <summary>
    /// 设置业务数据实体
    /// </summary>
    /// <param name="data"></param>
    public virtual void SetData(PageItemData data)
    {
        Data = data;
        OnClickHandler = data.OnClickHandler;
    }

    /// <summary>
    /// 重置为初始状态
    /// </summary>
    public virtual void Reset()
    {
        Data = null;
        OnClickHandler = null;
    }

    protected override void OnClick()
    {
        base.OnClick();
        if (OnClickHandler != null)
        {
            OnClickHandler(this, Data);

        }
    }

    protected override void OnEnter()
    {
        base.OnEnter();
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

}
