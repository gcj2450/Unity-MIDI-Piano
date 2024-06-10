using System;
using UnityEngine;
using DG.Tweening;

public class ViewBase : MonoBehaviour
{
    public string Name { get; set; }
    public bool Visible { get; private set; }
    public ShowViewEventArgs Args { get; protected set; }

    /// <summary>
    /// 初始化子容器集合以及窗体动画相关变量
    /// </summary>
    protected virtual void Awake()
    {
    }

    /// <summary>
    /// 其它初始化逻辑
    /// </summary>
    protected virtual void Start()
    {
    }

    /// <summary>
    /// 显示窗口
    /// </summary>
    /// <param name="args"></param>
    public virtual void Show(ShowViewEventArgs args)
    {
        if(Visible) return;
        Visible = true;
        Args = args;
        gameObject.SetActive(true);
        if (Args.ShowWithAnimation)
        {
            DoShowAnimation();
        }
    }

    /// <summary>
    /// 隐藏窗口
    /// </summary>
    public virtual void Hide()
    {
        if(!Visible) return;
        Visible = false;
        if (Args.ShowWithAnimation)
        {
            DoHideAnimation();
        }
        else
        {
            OnHideAnimationFinished();
        }
    }

    /// <summary>
    /// 隐藏动画结束后逻辑
    /// </summary>
    protected virtual void OnHideAnimationFinished()
    {
        if (Args.DestroyAutomatically)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void DoShowAnimation()
    {
       
        /*
        int fact = PlayerData.AnimationForward ? 1 : -1;
        gameObject.SetActive(Visible);
        Vector3 from;
        if (Math.Abs(transform.localPosition.x) > 10)
        {
            from = new Vector3(Screen.width*fact, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            from = Vector3.zero;
        }
        DOTween.To(() => from, x => transform.localPosition = x,
                    new Vector3(from.x - fact  * Screen.width, from.y, from.z), animationTime).SetAutoKill();
        */
    }

    protected virtual void DoHideAnimation()
    {
        OnHideAnimationFinished();
        /*
        Vector3 from;
        int fact = PlayerData.AnimationForward? 1 : -1;
        if (Math.Abs(transform.localPosition.x) > 10)
        {
            from = new Vector3( Screen.width*fact, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            from = Vector3.zero;
        }
        
        DOTween.To(() => from, x => transform.localPosition = x,
                 new Vector3(from.x - fact * Screen.width, from.y, from.z), animationTime-0.1f).OnComplete(OnAnimationFinished);
        */
    }

}
