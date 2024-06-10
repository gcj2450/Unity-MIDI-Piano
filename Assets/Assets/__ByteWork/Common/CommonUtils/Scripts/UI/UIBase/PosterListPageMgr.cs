using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SimpleTween;
/// <summary>
/// 页面列表管理器，用于生成多个页面列表
/// 根据当前栏目需求，提供对应类型的页面列表
/// 具有列表池管理功能
/// </summary>
public class PosterListPageMgr : MonoBehaviour
{
    /// <summary>
    ///X*X通用列表预制体
    /// </summary>
    public GameObject CommonPosterListPrefab = null;
    /// <summary>
    /// 已经实例化的所有页面预制体
    /// </summary>
    [HideInInspector]
    public GameObject PageInstance;

    /// <summary>
    /// 当前翻页的页面，一般指第二页及其以后的页面物体
    /// </summary>
    [HideInInspector]
    public AbstractBaseListPage curScrollPage = null;

    protected virtual void Awake()
    {
        List<PageItemData> Datas = new List<PageItemData>();
        for (int i = 0; i < 300; i++)
        {
            PageItemData pid = new PageItemData(i);
            Datas.Add(pid);
        }
        SetPageList(Datas);
    }

    protected virtual void Update()
    {
        //用于测试代码：
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            GotoNextPage();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            GotoPreviousPage();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Reset();
        }
    }

    /// <summary>
    /// 根据第一页和第二页布局样式，分别设置内容
    /// </summary>
    /// <param name="_firstPageType"></param>
    /// <param name="_secondPageType"></param>
	public virtual void SetPageList(List<PageItemData> posterItemDatas)
    {
        //首先重置当前正在显示的列表页
        if (PageInstance != null)
        {
            PageInstance.GetComponent<AbstractBaseListPage>().Reset();
            PageInstance.SetActive(false);
        }
        curScrollPage = null;

		GameObject first = GetCommonPageList();
        AbstractBaseListPage abPage = first.GetComponent<AbstractBaseListPage>();
        abPage.SetTotalCount(posterItemDatas.Count);
        abPage.AddData(posterItemDatas);
        curScrollPage = abPage;
    }

    public virtual void Reset()
    {
        if (PageInstance != null)
        {
            PageInstance.GetComponent<AbstractBaseListPage>().Reset();
            PageInstance.SetActive(false);
        }
        curScrollPage = null;
    }

    /// <summary>
    /// 获取X*X类型页面
    /// </summary>
    /// <param name="pageType"></param>
    /// <returns></returns>
    GameObject GetCommonPageList()
    {
        //根据X*X的类型生成一个通用列表页
        //作为上面特殊第一页的子物体
        if (PageInstance != null)
        {
            PageInstance.SetActive(true);
            return PageInstance;
        }
        else
        {
            GameObject tmp = GameObject.Instantiate(CommonPosterListPrefab) as GameObject;
            tmp.SetActive(true);
            tmp.transform.SetParent(transform);
            tmp.transform.localEulerAngles = Vector3.zero;
            tmp.transform.localScale = Vector3.one;
            tmp.transform.localPosition = Vector3.zero;
            PageInstance = tmp;
            return tmp;
        }
    }

    public virtual void GotoNextPage()
    {
        if (curScrollPage.IsAnimating)
            return;
        if (transform.localPosition.x > -10) //单独移动一次页面
        {
            Vector3 targetPos = new Vector3(-1045, transform.localPosition.y, transform.localPosition.z);
			SimpleTweener.AddTween(() => transform.localPosition, x => transform.localPosition = x, targetPos, 0.2f);
        }
        else
        {
            if (curScrollPage != null)
                curScrollPage.GotoNextPage();
        }
    }

    public virtual void GotoPreviousPage()
    {
        if (curScrollPage.IsAnimating)
            return;
        if (curScrollPage.CurrentStartIndex == 0)//单独移动一次页面
        {
            Vector3 targetPos = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
			SimpleTweener.AddTween(() => transform.localPosition, x => transform.localPosition = x, targetPos, 0.2f);
        }
        else
        {
            if (curScrollPage != null)
                curScrollPage.GotoPreviousPage();
        }
    }
}