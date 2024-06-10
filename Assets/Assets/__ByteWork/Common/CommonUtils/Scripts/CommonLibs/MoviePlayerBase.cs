using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// 通用播放器控制基类，针对不同核心播放器集成此类，
/// 然后覆写相应方法，实现具体功能
/// </summary>
public class MoviePlayerBase : MonoBehaviour
{
    /// <summary>
    /// 是否暂停
    /// </summary>
    public bool Paused { get; protected set; }
    /// <summary>
    /// 是否准备好
    /// </summary>
    public bool IsReady { get; protected set; }

    public bool IsLoop { get; set; }
    [HideInInspector]
    public int _StartPlayTime;

    /// <summary>
    /// 开始事件
    /// </summary>
    public Action OnStart;
    /// <summary>
    /// 结束事件
    /// </summary>
    public Action OnEnd;
    /// <summary>
    /// 报错事件
    /// </summary>
    public Action<int ,int> OnError;
    public Action<string > OnErrorString;
    /// <summary>
    /// 视频第一帧准备好事件
    /// </summary>
    public Action OnFirstFrameReady;

    public Action OnReady;

    /// <summary>
    /// 左侧屏幕
    /// </summary>
    [HideInInspector]
    public GameObject LeftScreen;
    /// <summary>
    /// 右侧屏幕
    /// </summary>
    [HideInInspector]
    public GameObject RightScreen;

    /// <summary>
    /// 屏幕默认纹理Texture
    /// </summary>
    public Texture _defaultTexture;

    public virtual void Awake()
    {
        //监听屏幕切换事件
        PlayerUtils.Instance.OnScreenChanged =ScreenChange;
    }
    /// <summary>
    /// 屏幕切换事件响应
    /// </summary>
    /// <param name="obj"></param>
    public virtual void ScreenChange(ScreenBase obj)
    {
        LeftScreen = obj.LScreen;
        RightScreen = obj.RScreen;
        SetScreenMesh();
    }

    public virtual void PlayReady(Texture2D arg0)
    {
        Debug.Log("BaseReady");
        if (OnReady != null)
        {
            OnReady();
        }
        else
        {
            Debug.Log("BaseReady  OnReady=null");
        }
        SetScreenSize();
    }

    public virtual void VideoFirstFrameReady()
    {
        Debug.Log("gcj: OnVideoFirstFrameReady");
        IsReady = true;
        if (OnFirstFrameReady != null)
            OnFirstFrameReady();
        SetScreenSize();
    }
    /// <summary>
    /// 播放结束回调
    /// </summary>
    public virtual void PlayEnd()
    {
        Debug.Log("gcj : PlayEnd");
        if (OnEnd!=null)
            OnEnd();
        if (!IsLoop)
        {
            ResetScreenTexture();
        }
    }
    public virtual void SetSeekBarValue(float val)
    {

    }
    public virtual float GetSeekBarValue()
    {
        return 0;
    }
    public virtual void PlayError(string _errorCode = "")
    {
        if (OnErrorString != null)
        {
            OnErrorString(_errorCode);
        }
    }
    public virtual void PlayError(int _errorCode, int _errorDescription)
    {
        if (OnError != null)
        {
            OnError(_errorCode, _errorDescription);
        }
    }

    /// <summary>
    /// 设置播放屏幕
    /// </summary>
    public virtual void SetScreenMesh()
    {
        LeftScreen = PlayerUtils.Instance.curScreen.GetComponent<ScreenBase>().LScreen;
        RightScreen = PlayerUtils.Instance.curScreen.GetComponent<ScreenBase>().RScreen;
    }

    /// <summary>
    /// 播放视频
    /// </summary>
    /// <param name="url">播放地址</param>
    /// <param name="startTime">开始时间</param>
    public virtual void PlayMovie(string url, int startTime=0)
    {
        Debug.Log("==============PlayMovie");
        PlayerUtils.Instance.SetScreenType(PlayerUtils.Instance.CurScreenType, PlayerUtils.Instance.CurDivideMode);
        SetScreenMesh();
        Reset();
        _StartPlayTime = startTime;
    }

    /// <summary>
    /// 重置屏幕纹理
    /// </summary>
    public virtual void ResetScreenTexture()
    {
        //var defaultLayer = LayerMask.NameToLayer("Default");
        //if(_defaultTexture!=null&& LeftScreen!=null)
        //    LeftScreen.gameObject.GetComponent<MeshRenderer>().material.mainTexture = _defaultTexture;
        //LeftScreen.gameObject.layer = defaultLayer;
        //LeftScreen.gameObject.SetActive(true);
        //if(_defaultTexture!=null&& RightScreen!=null)
        //    RightScreen.gameObject.GetComponent<MeshRenderer>().material.mainTexture = _defaultTexture;
        //RightScreen.gameObject.layer = defaultLayer;
        //RightScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 获取影片时长
    /// </summary>
    public virtual float GetDuration()
    {
        return 10000;
    }

    /// <summary>
    /// 获取当前播放进度
    /// </summary>
    /// <returns></returns>
    public virtual float GetSeekPosition()
    {
        return 0;
    }
    
    /// <summary>
    /// 设置播放进度，跳转到某一进度位置
    /// </summary>
    /// <param name="_seekPos">设置的进度位置</param>
    public virtual void SetSeekPosition(float _seekPos)
    {

    }

    /// <summary>
    /// 获取视频宽度
    /// </summary>
    /// <returns></returns>
    public virtual int GetVideoWidth()
    {
        return 1920;
    }

    /// <summary>
    /// 获取视频高度
    /// </summary>
    /// <returns></returns>
    public virtual int GetVideoHeight()
    {
        return 1080;
    }
    /// <summary>
    /// 设置屏幕尺寸
    /// </summary>
    public virtual void SetScreenSize()
    {
        PlayerUtils.Instance.ResizeScreen(LeftScreen, RightScreen, GetVideoWidth(), GetVideoHeight());
    }

    /// <summary>
    /// 继续播放
    /// </summary>
    public virtual void Resume()
    {
        Paused = false;
    }

    /// <summary>
    /// 暂停播放
    /// </summary>
    public virtual void Pause()
    {
        Paused = true;
    }

    /// <summary>
    /// 停止播放
    /// </summary>
    public virtual void Stop()
    {
        Paused = false;
    }

    public virtual void Reset()
    {
        Paused = false;
        IsReady = false;
    }
}
