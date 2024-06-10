using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// 播放控制基类，包含最基本的播放暂停以及进度显示
/// </summary>
public class VideoControlBase : MonoBehaviour
{
    public Slider proceSlider;

	public MoviePlayerBase scrMedia;

    public GameObject playBtn;
    public GameObject pauseBtn;

    public Text currentTime;
    public Text allTime;

    string fileName = "http://clips.vorwaerts-gmbh.de/VfE_html5.mp4";

    public Action<string> OnError;
    public Action OnReady;
    public Action OnEnd;
    protected virtual void Awake()
    {
        playBtn.GetComponent<Button>().onClick.AddListener(ContiuePlayVideo);
        pauseBtn.GetComponent<Button>().onClick.AddListener(PauseVideo);

    }
    protected virtual void Start()
    {
        PlayVideo(fileName);
    }

    public virtual void PlayVideo(string url)
    {
        RemoveVideoListen();
        proceSlider.value = 0;
        scrMedia.gameObject.SetActive(true);
		Debug.Log("----------url="+url);

		scrMedia.PlayMovie(url);
		InitVideoListen ();
    }

	/// <summary>
	/// 初始化视频播放事件
	/// </summary>
	public virtual void InitVideoListen(){
		Debug.Log ("--------------InitVideoListen");
		scrMedia.OnErrorString += PlayError;
		scrMedia.OnFirstFrameReady += PlayReady;
		scrMedia.OnReady += PlayReady;
		scrMedia.OnEnd += PlayEnd;
	}

    public virtual void PlayError(string obj)
    {
        if (OnError != null)
            OnError(obj);
        Debug.Log(obj);
    }

    /// <summary>
    /// 移除视频事件
    /// </summary>
    public virtual void RemoveVideoListen(){
        scrMedia.OnErrorString -= PlayError;
        scrMedia.OnFirstFrameReady -= PlayReady;
        scrMedia.OnReady -= PlayReady;
        scrMedia.OnEnd -= PlayEnd;
	}
    
    public virtual void PlayEnd()
    {
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        if (OnEnd != null)
            OnEnd();
        RemoveVideoListen();
    }
    bool beginUpdateTime = false;
    public virtual void PlayReady()
    {
		Debug.Log ("--------------VideoInit");
		SetVideoAllTime ();
        beginUpdateTime = true;
        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);
        if (OnReady!=null)
        {
            OnReady();
        }
    }

    public virtual void ContiuePlayVideo()
    {
        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);
        scrMedia.Resume();
    }
    public virtual void PauseVideo()
    {
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        scrMedia.Pause();
    }

    /// <summary>
    /// 把时间秒数转化成字符串00:00:00
    /// </summary>
    /// <param name="second">时间秒数</param>
    /// <returns>字符串00:00:00</returns>
    protected string TransTimeSecondIntToString(long value)
    {
		long second = value/1000;
        string str = "";
        try
        {
            long hour = second / 3600;
            long min = second % 3600 / 60;
            long sec = second % 60;
            if (hour < 10)
            {
                str += "0" + hour.ToString();
            }
            else
            {
                str += hour.ToString();
            }
            str += ":";
            if (min < 10)
            {
                str += "0" + min.ToString();
            }
            else
            {
                str += min.ToString();
            }
            str += ":";
            if (sec < 10)
            {
                str += "0" + sec.ToString();
            }
            else
            {
                str += sec.ToString();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Catch:" + ex.Message);
        }
        return str;
    }


	protected virtual void FixedUpdate(){
        if (beginUpdateTime)
        {
            SetVideoCurrentTime((long)scrMedia.GetSeekPosition());
            //进度条在SeekBarCtrl中会自动设置
            //SetSlidervalue((float)scrMedia.GetSeekPosition() / (float)scrMedia.GetDuration());
        }
	}
//	/// <summary>
//	/// 设置视频的时间格式等等
//	/// </summary>
//
	public void SetVideoAllTime(){
		allTime.text = TransTimeSecondIntToString ((long)scrMedia.GetDuration()).ToString ();
	}

	public void SetVideoCurrentTime(long value){
		currentTime.text = TransTimeSecondIntToString (value).ToString ();
	}
	public virtual void SetSlidervalue(float value){
		proceSlider.value = value;
	}
}
