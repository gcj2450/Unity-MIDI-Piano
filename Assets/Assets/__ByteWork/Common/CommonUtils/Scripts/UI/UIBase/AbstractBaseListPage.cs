using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 不带总页数的列表页抽象类
/// </summary>
public abstract class AbstractBaseListPage : MonoBehaviour
{
    [HideInInspector]
    public List<PageItemData> Datas = new List<PageItemData>();
    protected Stack<GameObject> ItemStack = new Stack<GameObject>();

    public List<PageItem> PageItems = new List<PageItem>();
    [HideInInspector]
    public bool IsAnimating;
    protected bool forward;
    protected Vector3 initialPositon;

    /// <summary>
    /// 当前显示的第一个节点的全局索引
    /// </summary>
    public int CurrentStartIndex { get; set; }

    /// <summary>
    /// 移动动画对象
    /// </summary>
    public Transform TweenTransform;

    /// <summary>
    /// 移动动画时间
    /// </summary>
    public float AnimationTime = 0.5f;
    /// <summary>
    /// 加载数据回调
    /// </summary>
    public Action NotifyGetData;

    /// <summary>
    /// 页变更回调(当前页，总页)
    /// </summary>
    public Action<int ,int> PageChanged;

    /// <summary>
    /// 初始化完，传回总页码
    /// </summary>
    public Action<int> InitCallback;

    /// <summary>
    /// 当前页面索引
    /// </summary>
    public int CurrentPageIndex { get; protected set; }

    /// <summary>
    /// 当前是否是第一页
    /// </summary>
    public virtual bool IsFirstPage
    {
        get { return CurrentPageIndex == 0; }
    }
    /// <summary>
    /// 页数
    /// </summary>
    public virtual int PageCount
    {
        get { return Mathf.CeilToInt((float)TotalItemCount / MaxPageItem); }
    }

    /// <summary>
    /// 当前是否是最后一页
    /// </summary>
    public virtual bool IsLastPage
    {
        get { return CurrentPageIndex == PageCount - 1; }
    }

    [Tooltip("true=横向滚动,false=纵向滚动")]
    public bool IsHorizontal = true;
    /// <summary>
    /// 每页显示的节点个数
    /// </summary>
    public int MaxPageItem = 12; //每页个数
    /// <summary>
    /// 总共节点数目
    /// </summary>
    public int TotalItemCount { get; protected set; }
    /// <summary>
    /// 每页显示列数
    /// </summary>
    public int ColumnCount = 4;

    public Vector2 PageSpacing = new Vector2(300, 300);    //页面间距

    /// <summary>
    /// 节点之间距离
    /// </summary>
    public Vector2 ItemSpacing = new Vector2(50, 50);

    /// <summary>
    /// 节点大小
    /// </summary>
    public Vector2 ItemSize = new Vector2(200, 320);

    /// <summary>
    /// 节点对象预设
    /// </summary>
    public GameObject ItemPrefab;

    /// <summary>
    /// 设置总数
    /// </summary>
    /// <param name="count"></param>
    public abstract void SetTotalCount(int count);
    /// <summary>
    /// 重置列表
    /// </summary>
    public virtual void Reset()
    {
    }
    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="data"></param>
    public virtual void AddData(List<PageItemData> data)
    {
    }

    /// <summary>
    /// 为每个Item设置数据
    /// </summary>
    public virtual void SetData()
    {

    }

    /// <summary>
    /// 刷新Items数据
    /// </summary>
    public virtual void RefreshItems()
    {

    }

    /// <summary>
    /// 翻到上一页
    /// </summary>
    public virtual void GotoNextPage()
    {
    }
    /// <summary>
    /// 翻到下一页
    /// </summary>
    public virtual void GotoPreviousPage()
    {
    }

    protected virtual void Awake()
    {
    }
    protected virtual void Start()
    {
    }
    protected virtual void Update()
    {
    }

    protected virtual Vector3 GetItemPos(int _id)
    {
        return Vector3.zero;
    }

    protected virtual float GetPageSize()
    {
        return 0;
    }

}
