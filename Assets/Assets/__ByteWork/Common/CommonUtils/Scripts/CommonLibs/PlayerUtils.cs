using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 屏幕控制工具类，首先管理所有屏幕预制，然后通过给出的屏幕类型
/// 生成或者激活相应屏幕。并可进行屏幕分屏模式的切换
/// </summary>
public class PlayerUtils : MonoBehaviour
{
    /// <summary>所有屏幕预制
    /// </summary>
    public List<GameObject> ScreenPrefabs = new List<GameObject>();
    public ScreenType CurScreenType = ScreenType.Pano;
    public ScreenDivideMode CurDivideMode = ScreenDivideMode.SingleScreen;
    public FlipType CurFlipType = FlipType.None;
    /// <summary>场景中的屏幕
    /// </summary>
    List<GameObject> SceneScreens = new List<GameObject>();
    //[HideInInspector]
    public GameObject curScreen;
    /// <summary>
    /// 屏幕切换事件
    /// </summary>
    public Action<ScreenBase> OnScreenChanged;
    [HideInInspector]
    public ScreenType tempScreenType = ScreenType.None;
    [HideInInspector]
    public ScreenDivideMode tempDivideMode = ScreenDivideMode.None;
    private static PlayerUtils _instance;
    public static PlayerUtils Instance
    {
        get
        {
            if (_instance == null)
            {
                var app = FindObjectOfType<PlayerUtils>();
                if (app == null)
                {
                    var appObject = new GameObject("PlayerUtils");
                    app = appObject.AddComponent<PlayerUtils>();
                }
                _instance = app;
            }

            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        //DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;


    }

    // Use this for initialization
    void Start()
    {
    }
#region OldCtrl
    /// <summary>
    /// 设置屏幕类型（平面/360度/半球/立方体）
    /// 从场景中找出一个匹配类型的屏幕，如果没找到就从预制列表中生成一个，并设置为当前屏幕，
    /// 然后调用事件OnScreenChanged 该事件需要在MoviePlayerBase中监听
    /// </summary>
    /// <param name="_screenType"></param>
    public void SetScreenType(ScreenType _screenType, ScreenDivideMode _sdvidmod)
    {
        //如果和当前的模式相同，返回
        if (_screenType == tempScreenType && _sdvidmod == tempDivideMode)
            return;
        GameObject newScreen;
        ScreenBase[] sbs = GameObject.FindObjectsOfType<ScreenBase>();
        foreach (var item in sbs)
        {
            if (!SceneScreens.Contains(item.gameObject))
                SceneScreens.Add(item.gameObject);
        }

        //从SceneScreens中查找合适的screen
        newScreen = SceneScreens.FirstOrDefault(
            p => p.GetComponent<ScreenBase>().MyScreenType == _screenType &&
            p.GetComponent<ScreenBase>().MyScreenMode == _sdvidmod);

        //如果没有找到，从预制中找到一个合适的screen，并生成
        if (newScreen == null)
        {
            GameObject tmp = ScreenPrefabs.FirstOrDefault(
                p => p.GetComponent<ScreenBase>().MyScreenType == _screenType &&
                p.GetComponent<ScreenBase>().MyScreenMode == _sdvidmod);

            newScreen = GameObject.Instantiate(tmp, Vector3.zero, Quaternion.identity) as GameObject;
        }

        if (curScreen == null) //如果当前屏幕为空（第一次启动的情况，设置为当前屏幕）
        {
            curScreen = newScreen;
        }
        else //如果当前屏幕不为空，首先将其隐藏，然后设置当前屏幕
        {
            if (curScreen != newScreen)
            {
                
                curScreen.SetActive(false);
                curScreen.transform.localScale = new Vector3(1, 1, 1);
                SceneScreens.Add(curScreen);
            }
            curScreen = newScreen;
            curScreen.SetActive(true);
        }
        //Debug.Log("VVVVVVVVVV");
        if (OnScreenChanged != null)
        {
            OnScreenChanged(curScreen.GetComponent<ScreenBase>());
            //Debug.Log("GGGGGGG");
        }
        else
        {
            Debug.Log("OnScreenChanged=null");
        }
        CurScreenType = _screenType;
        CurDivideMode = _sdvidmod;

        tempScreenType = _screenType;
        tempDivideMode = _sdvidmod;
        //if (_screenType == ScreenType.Plane)
        //{
        //    curScreen.transform.localPosition = new Vector3(0, 1.2f, 7f);
        //}
        FlipScreen(CurFlipType);
    }

    public void FlipScreen(FlipType _fType)
    {
        if (curScreen == null)
        {
            Debug.Log("curScreen is null can not flip");
            return;
        }
        Vector3 oldScale = curScreen.transform.localScale;
        switch (_fType)
        {
            case FlipType.None:
                curScreen.transform.localScale = Vector3.one;

                break;
            case FlipType.Horizontal:
                curScreen.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case FlipType.Vertical:
                curScreen.transform.localScale = new Vector3(1, -1, 1);
                break;
            case FlipType.VerticalHorizontal:
                curScreen.transform.localScale = new Vector3(-1, -1, 1);
                break;
            default:
                break;
        }
        CurFlipType = _fType;

    }

    public void SetScreenScale(float _value)
    {
        switch (CurFlipType)
        {
            case FlipType.None:
                curScreen.transform.localScale = new Vector3(_value, _value, _value);

                break;
            case FlipType.Horizontal:
                curScreen.transform.localScale = new Vector3(-_value, _value, _value);
                break;
            case FlipType.Vertical:
                curScreen.transform.localScale = new Vector3(_value, -_value, _value);
                break;
            case FlipType.VerticalHorizontal:
                curScreen.transform.localScale = new Vector3(-_value,- _value, _value);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 设置屏幕分屏模式（单屏/左右屏/上下屏）
    /// </summary>
    /// <param name="_lScreen">左屏幕</param>
    /// <param name="_rScreen">右屏幕</param>
    /// <param name="_sMode">屏幕模式</param>
    /// <param name="_flipY">是否上下反转</param>
    //public void SetScreeMode(GameObject _lScreen, GameObject _rScreen, ScreenDivideMode _sMode, bool _flipY)
    //{
    //    if (_lScreen == null || _rScreen == null)
    //        return;
    //    Vector2 scale = Vector2.one;
    //    Vector2 vSizeOff = new Vector2(0.0f, 0.0f);
    //    switch (_sMode)
    //    {
    //        case ScreenDivideMode.SingleScreen:
    //            if (_flipY)
    //                scale.y = -1;
    //            _lScreen.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", scale);
    //            _lScreen.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", vSizeOff);
    //            _lScreen.GetComponent<MeshRenderer>().material.SetFloat("_URight", 1f);
    //            _rScreen.GetComponent<MeshRenderer>().material.SetFloat("_ULeft", 0f);

    //            _lScreen.GetComponent<MeshRenderer>().material.SetFloat("_VBottom", -1f);

    //            _rScreen.GetComponent<MeshRenderer>().material.SetFloat("_VTop", -0f);

    //            _rScreen.SetActive(false);
    //            break;
    //        case ScreenDivideMode.LRScreen:
    //            if (_flipY)
    //            {
    //                _lScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(0.5f, -1);
    //                _rScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(0.5f, -1);
    //                _rScreen.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
    //            }
    //            else
    //            {
    //                _lScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(0.5f, 1);
    //                _rScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(0.5f, 1);
    //                _rScreen.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
    //            }
    //            _rScreen.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.5f, 0);

    //            _lScreen.GetComponent<MeshRenderer>().material.SetFloat("_URight", 0.5f);

    //            _rScreen.GetComponent<MeshRenderer>().material.SetFloat("_ULeft", 0.5f);

    //            _lScreen.GetComponent<MeshRenderer>().material.SetFloat("_VBottom", -1f);

    //            _rScreen.GetComponent<MeshRenderer>().material.SetFloat("_VTop", -0f);

    //            _rScreen.SetActive(true);

    //            break;
    //        case ScreenDivideMode.UDScreen:

    //            if (_flipY)
    //            {
    //                _lScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1f, -0.5f);
    //                _rScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1f, -0.5f);
    //                _rScreen.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.5f));
    //            }
    //            else
    //            {
    //                _lScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1f, 0.5f);
    //                _rScreen.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1f, 0.5f);
    //                _rScreen.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.5f));
    //            }
    //            _rScreen.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0, -0.5f);

    //            _lScreen.GetComponent<MeshRenderer>().material.SetFloat("_URight", 1f);

    //            _rScreen.GetComponent<MeshRenderer>().material.SetFloat("_ULeft", 0f);

    //            _lScreen.GetComponent<MeshRenderer>().material.SetFloat("_VBottom", -0.5f);

    //            _rScreen.GetComponent<MeshRenderer>().material.SetFloat("_VTop", -0.5f);

    //            _rScreen.SetActive(true);
    //            break;
    //        default:
    //            break;
    //    }
    //}
    /// <summary>视频缩放
    /// </summary>
    /// <param name="_lScreen">左侧屏幕</param>
    /// <param name="_rScreen">右侧屏幕</param>
    /// <param name="_width">当前视频宽</param>
    /// <param name="_height">当前视频高</param>
    public void ResizeScreen(GameObject _lScreen, GameObject _rScreen, int _width, int _height)
    {
        if (_lScreen != null)
        {
            _lScreen.GetComponent<Renderer>().material.SetFloat("_VOffset", 0);
            _lScreen.GetComponent<Renderer>().material.SetFloat("_UOffset", 0);
        }
        if (_rScreen != null)
        {
            _rScreen.GetComponent<Renderer>().material.SetFloat("_VOffset", 0);
            _rScreen.GetComponent<Renderer>().material.SetFloat("_UOffset", 0);
        }
        float screenRatio = 19.2f / 10.8f;
        Debug.Log("width : " + _width + "  height : " + _height);
        //if (_width >= _height)    //横幅或者方形视频，大多数都是这样
        {
            float videoR = (float)_width / _height;
            if (videoR >= screenRatio) //说明比1920*1080更宽
            {
                Debug.Log("比1920*1080更宽");
                float _fheight = (1920.0f / _width) * _height;  //以宽度为基准对高度进行缩放
                float _vo = ((1080.0f - _fheight) / 1080.0f) * 0.5f;    //计算出高度缩放后需要填充黑色的偏移值
                if (_lScreen != null)
                    _lScreen.GetComponent<Renderer>().material.SetFloat("_VOffset", _vo);
                if (_rScreen != null)
                    _rScreen.GetComponent<Renderer>().material.SetFloat("_VOffset", _vo);
            }
            else
            {
                Debug.Log("比1920*1080更高");
                float _fwidth = (1080.0f / _height) * _width;
                float _ho = ((1920.0f - _fwidth) / 1920.0f) * 0.5f;
                if (_lScreen != null)
                    _lScreen.GetComponent<Renderer>().material.SetFloat("_UOffset", _ho);
                if (_rScreen != null)
                    _rScreen.GetComponent<Renderer>().material.SetFloat("_UOffset", _ho);
            }

            Debug.Log(screenRatio + "    " + videoR);
        }
    }
    /// <summary>将int值转换为时间格式
    /// </summary>
    /// <param name="nSeek"></param>
    /// <returns></returns>
    public string FormatIntToTimeStr(int nSeek)
    {
        int hour = 0;
        int minute = 0;
        int second = nSeek;
        const int secondsPerMinute = 60;

        if (second > secondsPerMinute)
        {
            minute = second / secondsPerMinute;
            second = second % secondsPerMinute;
        }
        if (minute >= secondsPerMinute)
        {
            hour = minute / secondsPerMinute;
            minute = minute % secondsPerMinute;
        }
        string strhour = hour < 10 ? "0" + hour : hour.ToString();
        string strMinute = minute < 10 ? "0" + minute : minute.ToString();
        string strSecond = second < 10 ? "0" + second : second.ToString();
        return strhour + ":" + strMinute + ":" + strSecond;
    }
#endregion




}

/// <summary>
/// 屏幕分屏模式（单屏/左右屏/上下屏）
/// </summary>
public enum ScreenDivideMode
{
    None,
    SingleScreen,
    LRScreen,
    UDScreen
}

/// <summary>
/// 屏幕类型（平面/360全景/180度半球/立方体）
/// </summary>
public enum ScreenType
{
    None,
    Plane,
    Pano,
    Dome,
    Cube
}

public enum FlipType
{
    None,
    Horizontal,
    Vertical,
    VerticalHorizontal
}