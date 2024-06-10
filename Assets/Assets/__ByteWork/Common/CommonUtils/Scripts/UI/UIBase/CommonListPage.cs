using System;
using System.Collections;
using System.Collections.Generic;
using SimpleTween;
using UnityEngine;
/// <summary>
/// 动态无限滚动列表，支持横向/纵向滚动
/// 不参与业务处理
/// </summary>
public class CommonListPage : AbstractBaseListPage
{
    /// <summary>
    /// 执行初始化，记录初始位置等
    /// </summary>
    protected override void Awake()
    {
        initialPositon = transform.localPosition;
        if (TweenTransform == null)
            TweenTransform = transform;
        //SetTotalCount(10);
    }

    protected override void Update()
    {
        //用于测试代码：
        //if (Input.GetKeyUp(KeyCode.RightArrow))
        //{
        //    GotoNextPage();
        //    GetInitialPosition();
        //}
        //if (Input.GetKeyUp(KeyCode.LeftArrow))
        //{
        //    GotoPreviousPage();
        //}
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    Reset();
        //}
    }

    /// <summary>
    /// 设置列表节点总数
    /// </summary>
    public override void SetTotalCount(int count)
    {
        //VRLogger.Log("SetTotalCount" + count);
        CurrentPageIndex = -1;
        TotalItemCount = count;
        var firstPageCount = Mathf.Min(TotalItemCount, MaxPageItem);
        for (int i = 0; i < firstPageCount; i++)
        {
            int index = CurrentStartIndex + i;
            PageItems.Add(CreatePageItem(index));
        }
        //VRLogger.Log("CurrentStartIndex: " + CurrentStartIndex);
        forward = true;
        OnTweenFinished();
        if (InitCallback != null)
        {
            InitCallback(PageCount);
        }
    }

    /// <summary>
    /// 重设列表控件
    /// </summary>
    public override void Reset()
    {
        Datas.Clear();
        RemoveAllPageItems();
        CurrentStartIndex = 0;
        TotalItemCount = 0;
        //ClearItems();
        CurrentPageIndex = 0;
        TotalItemCount = 0;
        NotifyPageChanged();
        transform.localPosition = initialPositon;
    }

    /// <summary>
    /// 添加业务数据对象
    /// </summary>
    public override void AddData(List<PageItemData> pDatas)
    {
        Datas.AddRange(pDatas);
        SetData();
    }

    /// <summary>
    /// 移动到下一页
    /// </summary>
    public override void GotoNextPage()
    {
        if (IsAnimating) return;
        if (CurrentStartIndex + MaxPageItem < TotalItemCount)
        {
            AnimateToNextPage();
        }
    }

    /// <summary>
    /// 移动到上一页
    /// </summary>
    public override void GotoPreviousPage()
    {
        if (IsAnimating) return;
        if (CurrentStartIndex > 0)
        {
            AnimateToPreviousPage();
        }
    }

    protected virtual void AnimateToNextPage()
    {
        IsAnimating = true;
        forward = true;
        var moveCount = Mathf.Min(TotalItemCount - CurrentStartIndex - MaxPageItem, MaxPageItem);
        //float animTime = (float)moveCount / MaxPageItem * AnimationTime;
        if (IsHorizontal)
        {
            var moveMount = GetPageSize() + PageSpacing.x;
            Vector3 targetPos = new Vector3(
                TweenTransform.localPosition.x - moveMount,
                TweenTransform.localPosition.y,
                TweenTransform.localPosition.z);
			SimpleTweener.AddTween(() => TweenTransform.transform.localPosition, x => 
			                       TweenTransform.transform.localPosition = x, targetPos, AnimationTime).OnCompleted(OnTweenFinished);
        }
        else
        {
            var moveMount = GetPageSize() + PageSpacing.y;
            Vector3 targetPos = new Vector3(TweenTransform.localPosition.x,
                TweenTransform.localPosition.y + moveMount,
                TweenTransform.localPosition.z);
			SimpleTweener.AddTween(() => TweenTransform.transform.localPosition, x =>
			                       TweenTransform.transform.localPosition = x, targetPos, AnimationTime).OnCompleted(OnTweenFinished);
        }

        CurrentStartIndex += moveCount;
        if (CurrentStartIndex + 2 * MaxPageItem > Datas.Count && Datas.Count < TotalItemCount)
        {
            //Debug.Log("NNNNNNNNNN");
            if (NotifyGetData != null)
            {
                NotifyGetData();
            }
        }
        //ClearItems();
    }

    protected virtual void AnimateToPreviousPage()
    {
        IsAnimating = true;
        forward = false;
        var moveCount = Mathf.Min(CurrentStartIndex, MaxPageItem);
        //float animTime = (float)moveCount / MaxPageItem * AnimationTime;
        if (IsHorizontal)
        {
            var moveMount = GetPageSize() + PageSpacing.x;
            Vector3 targetPos = new Vector3(TweenTransform.localPosition.x + moveMount,
                TweenTransform.localPosition.y,
                TweenTransform.localPosition.z);
			SimpleTweener.AddTween(() => TweenTransform.transform.localPosition, x =>
                                   TweenTransform.transform.localPosition = x, targetPos, AnimationTime).OnCompleted(OnTweenFinished);
        }
        else
        {
            var moveMount = GetPageSize() + PageSpacing.y;
            Vector3 targetPos = new Vector3(TweenTransform.localPosition.x,
                TweenTransform.localPosition.y - moveMount,
                TweenTransform.localPosition.z);
			SimpleTweener.AddTween(() => TweenTransform.transform.localPosition, x =>
                                   TweenTransform.transform.localPosition = x, targetPos, AnimationTime).OnCompleted(OnTweenFinished);
        }
        CurrentStartIndex -= moveCount;
        //ClearItems();
    }

    protected virtual void OnTweenFinished()
    {
        if (TotalItemCount > 0)
        {
            if (forward)
            {
                var prepareCount = Mathf.Min(TotalItemCount - CurrentStartIndex - MaxPageItem, MaxPageItem);
                for (int i = 0; i < prepareCount; i++)
                {
                    int index = CurrentStartIndex + MaxPageItem + i;
                    PageItems.Add(CreatePageItem(index));
                }
                CurrentPageIndex++;
                NotifyPageChanged();
            }
            else
            {
                var prepareCount = Mathf.Min(CurrentStartIndex, MaxPageItem);
                for (int i = 0; i < prepareCount; i++)
                {
                    int index = CurrentStartIndex - 1 - i;
                    PageItems.Add(CreatePageItem(index));
                }
                CurrentPageIndex--;
                NotifyPageChanged();
            }
        }
        RefreshItems();
        IsAnimating = false;
        //VRLogger.Log("CurrentStartIndex: " + CurrentStartIndex);
    }

    public override void RefreshItems()
    {
        var pageItems = new List<PageItem>();
        foreach (var pageItem in PageItems)
        {
            if (pageItem.GlobalIndex >= CurrentStartIndex && pageItem.GlobalIndex < CurrentStartIndex + MaxPageItem)
            {
                pageItems.Add(pageItem);
            }
        }

        pageItems.Sort((x, y) => x.GlobalIndex.CompareTo(y.GlobalIndex));
        for (int i = 0; i < pageItems.Count; i++)
        {
            var item = pageItems[i];
            item.Id = i;
            //Items.Add(item);
        }

        for (int i = PageItems.Count - 1; i >= 0; i--)
        {
            var pageItem = PageItems[i];
            if (pageItem.GlobalIndex >= CurrentStartIndex + 2 * MaxPageItem ||
                pageItem.GlobalIndex < CurrentStartIndex - MaxPageItem)
            {
                PageItems.Remove(pageItem);
                DestroyPageItem(pageItem);
            }
        }

        SetData();
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

    public override void SetData()
    {
        foreach (PageItem pageItem in PageItems)
        {
            if (pageItem.Data == null && pageItem.GlobalIndex < Datas.Count)
            {
                pageItem.SetData(Datas[pageItem.GlobalIndex]);
            }
        }
    }

    //public void SetItmeData(object data)
    //{
    //    foreach (var pageItem in _pageItems)
    //    {
    //        if (pageItem.Data != null && data == pageItem.Data && pageItem.GlobalIndex < _datas.Count)
    //        {
    //            pageItem.SetData(_datas[pageItem.GlobalIndex]);
    //        }
    //    }
    //}
    protected virtual PageItem CreatePageItem(int index)
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
        var pageItem = temp.GetComponent<PageItem>();
        pageItem.Initialize(index);
        temp.transform.SetParent(transform, false);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = GetItemPos(index);
        temp.SetActive(true);
        return pageItem;
    }

    /// <summary>
    /// 通过Id计算出Item所在位置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected override Vector3 GetItemPos(int id)
    {
        int PageId = Mathf.FloorToInt(id / MaxPageItem);
        int index = id % MaxPageItem;

        int rowId = Mathf.FloorToInt(index / ColumnCount);
        int columId = index % ColumnCount;
        if (IsHorizontal)
        {
            float Posx = PageId * (GetPageSize() + PageSpacing.x) + columId * (ItemSize.x + ItemSpacing.x) + ItemSize.x * 0.5f;
            float PosY = rowId * (ItemSize.y + ItemSpacing.y) + ItemSize.y * 0.5f;
            return new Vector3(Posx, -PosY, 0);
        }
        else
        {
            float Posx = columId * (ItemSize.x + ItemSpacing.x) + ItemSize.x * 0.5f;
            float PosY = PageId * (GetPageSize() + PageSpacing.y) + rowId * (ItemSize.y + ItemSpacing.y) + ItemSize.y * 0.5f;
            return new Vector3(Posx, -PosY, 0);
        }
    }

    protected override float GetPageSize()
    {
        if (IsHorizontal)
        {
            return (ItemSize.x + ItemSpacing.x) * ColumnCount;
        }
        else
        {
            return (ItemSize.y + ItemSpacing.y) *Mathf.CeilToInt((float)MaxPageItem / ColumnCount);
        }
    }

    protected virtual void DestroyPageItem(PageItem item)
    {
        item.Data = null;
        item.Reset();
        item.gameObject.SetActive(false);
        ItemStack.Push(item.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void NotifyPageChanged()
    {
        //VRLogger.Log("NotifyPageChanged");
        if (PageChanged != null)
        {
            PageChanged(CurrentPageIndex,PageCount);
        }
    }

    /// <summary>
    /// 计算出当前列表页初始位置，可以不用这个，在场景中手动调节位置
    /// </summary>
    /// <returns></returns>
    protected virtual Vector2 GetInitialPosition()
    {
        float width = ItemSize.x * ColumnCount * 0.5f + ItemSpacing.x * (ColumnCount - 1) * 0.5f;

        float height = ItemSize.y * (float)MaxPageItem / ColumnCount * 0.5f +
            ItemSpacing.y * ((float)MaxPageItem / ColumnCount - 1) * 0.5f;

        return new Vector2(width, height);
    }

}
