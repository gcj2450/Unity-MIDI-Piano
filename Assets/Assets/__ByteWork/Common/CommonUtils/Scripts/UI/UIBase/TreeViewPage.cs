using SimpleTween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 需要提前知道总数的无限滚动列表
/// </summary>
public class TreeViewPage : MonoBehaviour
{
    public TreeViewItem MaskeRoot;
    //父节点蒙板的尺寸
    public Vector2 MaskSize;
    [Tooltip("true=横向滚动,false=纵向滚动")]
    public bool IsHorizontal = true;
    /// <summary>
    /// 移动动画时间
    /// </summary>
    public float AnimationTime = 0.5f;
    [HideInInspector]
    public List<PageItemData> Datas = new List<PageItemData>();
    //缓存中的Items
    protected Stack<GameObject> ItemStack = new Stack<GameObject>();
    //当前在用的Items
    public List<TreeViewItem> PageItems = new List<TreeViewItem>();
    /// <summary>
    /// 初始化完，传回当前第一个索引
    /// </summary>
    public Action<int> InitCallback;
    /// <summary>
    /// 页变更回调
    /// </summary>
    public Action PageChanged;
    /// <summary>
    /// 总共节点数目
    /// </summary>
    public int TotalItemCount { get; protected set; }
    /// <summary>
    /// 用于循环的最大Item数(这个数根据遮罩尺寸而定，并排排列完超出遮罩范围即可)
    /// </summary>
    public int MaxInitItemCount = 12;
    /// <summary>
    /// 并排显示数(横排：行数，纵排：列数)
    /// </summary>
    public int AlignCount = 2;
    /// <summary>
    /// 当前显示的第一个节点的全局索引
    /// </summary>
    public int CurrentStartIndex { get; set; }
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
    /// 向前翻动（横排指根节点向左移动，竖排指根节点向上移动）
    /// </summary>
    public bool forward;
    [HideInInspector]
    public bool IsAnimating;
    Vector3 initPos;
    //横排或者竖排的最大位置
    Vector2 MaxSizeRect;

    void Awake()
    {
        initPos = transform.localPosition;
        if (MaskeRoot == null)
            MaskeRoot = transform.parent.GetComponent<TreeViewItem>();
        MaskeRoot.OnBeginDragEvt += OnItemBeginDrag;
        MaskeRoot.OnDragEvt += OnItemDrag;
        MaskeRoot.OnEndDragEvt += OnItemEndDrag;
    }

    /// <summary>
    /// 设置列表节点总数
    /// </summary>
    public void SetTotalCount(int count)
    {
        TotalItemCount = count;
        for (int i = 0; i < MaxInitItemCount; i++)
        {
            int index = CurrentStartIndex + i;
            PageItems.Add(CreatePageItem(index));
        }
        if (IsHorizontal)
        {
            //根据总数计算出Item排列完的最大范围
            MaxSizeRect = new Vector2(0.5f * MaskSize.x - Mathf.CeilToInt(TotalItemCount / (float)AlignCount) * (ItemSize.x + ItemSpacing.x),
                                                        -0.5f * MaskSize.y + AlignCount * (ItemSize.y + ItemSpacing.y));
        }
        else
        {
            //根据总数计算出Item排列完的最大范围
            MaxSizeRect = new Vector2(0.5f * MaskSize.x - AlignCount * (ItemSize.x + ItemSpacing.x),
                                                        -0.5f * MaskSize.y + Mathf.CeilToInt(TotalItemCount / ((float)AlignCount)) * (ItemSize.y + ItemSpacing.y));
        }

        //VRLogger.Log("CurrentStartIndex: " + CurrentStartIndex);
        forward = true;
        if (InitCallback != null)
        {
            InitCallback(CurrentStartIndex);
        }
    }

    /// <summary>
    /// 重设列表控件
    /// </summary>
    public void Reset()
    {
        Datas.Clear();
        RemoveAllPageItems();
    }

    /// <summary>
    /// 添加业务数据对象
    /// </summary>
    public void AddData(List<PageItemData> pDatas)
    {
        Datas.AddRange(pDatas);
        SetData();
    }

    public void SetData()
    {
        foreach (TreeViewItem pageItem in PageItems)
        {
            if (pageItem.Data == null && pageItem.GlobalIndex < Datas.Count)
            {
                pageItem.SetData(Datas[pageItem.GlobalIndex]);
                pageItem.OnBeginDragEvt = OnItemBeginDrag;
                pageItem.OnDragEvt = OnItemDrag;
                pageItem.OnEndDragEvt = OnItemEndDrag;
            }
        }
    }

    /// <summary>
    /// 列表项开始被拖动
    /// </summary>
    /// <param name="obj"></param>
    private void OnItemBeginDrag(PointerEventData obj)
    {
    }

    /// <summary>
    /// 列表项拖动事件
    /// </summary>
    /// <param name="obj"></param>
    private void OnItemDrag(PointerEventData obj)
    {
        if (IsHorizontal)
        {
            transform.localPosition += new Vector3(obj.delta.x, 0, 0);
            if (obj.delta.x < 0)
            {
                forward = true;
            }
            else if (obj.delta.x > 0)
            {
                forward = false;
            }
        }
        else
        {
            transform.localPosition += new Vector3(0, obj.delta.y, 0);
            if (obj.delta.y < 0)
            {
                forward = false;
            }
            else if (obj.delta.y > 0)
            {
                forward = true;
            }
        }
        //UpdateItem();
        foreach (var item in PageItems)
        {
            UpdateItemPosAndIndex(item);
        }
    }

    /// <summary>
    /// 列表项结束被拖动
    /// </summary>
    /// <param name="obj"></param>
    private void OnItemEndDrag(PointerEventData obj)
    {

        if (IsHorizontal)
        {
            Vector3 targetPos = transform.localPosition + new Vector3(obj.delta.x * 5, 0, 0);
            SimpleTweener.AddTween(() => transform.localPosition, x =>
                                   transform.localPosition = x, targetPos, AnimationTime).OnCompleted(OnTweenFinished);
        }
        else
        {
            Vector3 targetPos = transform.localPosition + new Vector3(0, obj.delta.y * 5, 0);
            SimpleTweener.AddTween(() => transform.localPosition, x =>
                                   transform.localPosition = x, targetPos, AnimationTime).OnCompleted(OnTweenFinished);
        }

    }
    /// <summary>
    /// 根据位置逐个计算一下Item是否需要更新数据
    /// </summary>
    /// <param name="item"></param>
    void UpdateItemPosAndIndex(TreeViewItem item)
    {
        if (IsHorizontal)
        {
            //横排
            if (forward)
            {
                //如果该Item位置已经超出一个边界的宽度，则将其循环利用，用于展示下一个Item
                if (item.transform.localPosition.x + transform.localPosition.x < -MaskSize.x)
                {
                    ShowNextItem(item);
                }
            }
            else
            {
                if (item.transform.localPosition.x + transform.localPosition.x > MaskSize.x)
                {
                    ShowPrevItem(item);
                }
            }
        }
        else
        {
            //竖排
            if (forward)
            {
                //如果该Item位置已经超出一个边界的宽度，则将其循环利用，用于展示下一个Item
                if (item.transform.localPosition.y + transform.localPosition.y > MaskSize.y)
                {
                    ShowNextItem(item);
                }
            }
            else
            {
                if (item.transform.localPosition.y + transform.localPosition.y < -MaskSize.y)
                {
                    ShowPrevItem(item);
                }
            }
        }
    }

    //循环展示下一个Item
    public void ShowNextItem(TreeViewItem item)
    {
        if (CurrentStartIndex + MaxInitItemCount < TotalItemCount)
        {
            item.transform.localPosition = GetItemPos(CurrentStartIndex + MaxInitItemCount);
            item.Initialize(CurrentStartIndex + MaxInitItemCount);
            item.SetData(Datas[item.GlobalIndex]);
            item.name = item.GlobalIndex.ToString();
            item.transform.SetAsLastSibling();
            CurrentStartIndex++;
            //Debug.Log(CurrentStartIndex);
        }
    }

    //循环展示前一个Item
    public void ShowPrevItem(TreeViewItem item)
    {
        if (CurrentStartIndex > 0)
        {
            item.transform.localPosition = GetItemPos(CurrentStartIndex - 1);
            item.Initialize(CurrentStartIndex - 1);
            item.SetData(Datas[item.GlobalIndex]);
            item.name = item.GlobalIndex.ToString();
            item.transform.SetAsFirstSibling();
            CurrentStartIndex--;
            //Debug.Log(CurrentStartIndex);
        }
    }

    //移动完成后，计算一下位置，防止超出边界
    void OnTweenFinished()
    {
        foreach (var item in PageItems)
        {
            UpdateItemPosAndIndex(item);
        }
        if (IsHorizontal)
        {

            if (transform.localPosition.x < MaxSizeRect.x)
            {
                Vector3 targetPos = new Vector3(MaxSizeRect.x, transform.localPosition.y, transform.localPosition.z);
                SimpleTweener.AddTween(() => transform.localPosition, x =>
                                       transform.localPosition = x, targetPos, AnimationTime);
            }
            if (transform.localPosition.x > initPos.x)
            {
                SimpleTweener.AddTween(() => transform.localPosition, x =>
                                       transform.localPosition = x, initPos, AnimationTime);
            }
        }
        else
        {
            if (transform.localPosition.y > MaxSizeRect.y)
            {
                Vector3 targetPos = new Vector3(transform.localPosition.x, MaxSizeRect.y, transform.localPosition.z);
                SimpleTweener.AddTween(() => transform.localPosition, x =>
                                       transform.localPosition = x, targetPos, AnimationTime);
            }
            if (transform.localPosition.y < initPos.y)
            {
                SimpleTweener.AddTween(() => transform.localPosition, x =>
                                       transform.localPosition = x, initPos, AnimationTime);
            }
        }
    }

    protected virtual void NotifyPageChanged()
    {
        //VRLogger.Log("NotifyPageChanged");
        if (PageChanged != null)
        {
            PageChanged();
        }
    }

    protected void RemoveAllPageItems()
    {
        for (int i = PageItems.Count - 1; i >= 0; i--)
        {
            var pageItem = PageItems[i];
            PageItems.Remove(pageItem);
            DestroyPageItem(pageItem);
        }
    }

    protected virtual TreeViewItem CreatePageItem(int index)
    {
        if (ItemStack.Count == 0)
        {
            GameObject go = Instantiate(ItemPrefab);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActive(false);
            ItemStack.Push(go);
        }
        GameObject temp = ItemStack.Pop();
        var pageItem = temp.GetComponent<TreeViewItem>();
        pageItem.Initialize(index);
        temp.transform.SetParent(transform, false);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = GetItemPos(index);
        temp.SetActive(true);
        temp.name = index.ToString();
        return pageItem;
    }

    /// <summary>
    /// 通过Id计算出Item所在位置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected Vector3 GetItemPos(int index)
    {
        if (IsHorizontal)
        {
            float Posx = Mathf.FloorToInt((index) / (float)AlignCount) * (ItemSize.x + ItemSpacing.x) + 0.5f*ItemSize.x;
            float PosY = ((index) % AlignCount) * (ItemSize.y + ItemSpacing.y) + 0.5f*ItemSize.y;
            return new Vector3(Posx, -PosY, 0);
        }
        else
        {
            float Posx = ((index) % AlignCount) * (ItemSize.x + ItemSpacing.x) + 0.5f * ItemSize.x;
            float PosY = Mathf.FloorToInt((index) / (float)AlignCount) * (ItemSize.y + ItemSpacing.y) +0.5f* ItemSize.y;
            return new Vector3(Posx, -PosY, 0);
        }
    }

    protected virtual void DestroyPageItem(TreeViewItem item)
    {
        item.Data = null;
        item.gameObject.SetActive(false);
        ItemStack.Push(item.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnItemDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnItemBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag(eventData);
    }
}
