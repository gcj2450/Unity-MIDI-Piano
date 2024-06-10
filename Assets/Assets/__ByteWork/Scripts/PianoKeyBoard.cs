using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PianoKeyBoard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 正常图
    /// </summary>
    public Sprite NormalSprite;
   
     /// <summary>
    /// 按下图
    /// </summary>
    public Sprite PressedSprite;

    /// <summary>
    /// 按键Image
    /// </summary>
    public Image KeyImg;

    /// <summary>
    /// 键索引(0-87)
    /// </summary>
    public int ID = 0;
    //public KeyStep MyKeyStep=KeyStep.A;
    //public int MyKeyOctave = 0;
    /// <summary>
    /// 该按键AudioSource
    /// </summary>
    AudioSource keyAs;
    void Awake()
    {
        keyAs = GetComponent<AudioSource>();
        App.Instance.EventManager.RegisterHandler(
        EventNames.OnMusicSignCheck.ToString(), HandlePlayMusic
            );

        App.Instance.EventManager.RegisterHandler(
        EventNames.OnMusicSignExit.ToString(), HandleStopMusic
            );
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void HandlePlayMusic(ChangeUIEventArgs args)
    {
        
        int keyId = (int)args.Data;
        Debug.Log(keyId);
        if(keyId==ID)
        {
            PlayKeySound();
        }
    }

    void HandleStopMusic(ChangeUIEventArgs args)
    {

        int keyId = (int)args.Data;
        Debug.Log(keyId);
        if (keyId == ID)
        {
            SetNormalState();
        }
    }

    /// <summary>
    /// 播放该键声音
    /// </summary>
    public void PlayKeySound()
    {
        //Debug.Log("Play: " + ID);
        SetPressedState();
        if (keyAs.isPlaying)
            keyAs.Stop();
        keyAs.PlayOneShot(SoundMgr.Instance.KeySounds[ID]);
        //SoundMgr.Instance.PlaySound(SoundMgr.Instance.KeySounds[ID]);
        //Debug.Log(ID);
    }

    /// <summary>
    /// 光标按下事件
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayKeySound();
    }

    /// <summary>
    /// 光标抬起事件
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        SetNormalState();
    }

    /// <summary>
    /// 光标进入事件
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SoundMgr.Instance.IsMouseDown)
        {
            PlayKeySound();
        }

    }

    /// <summary>
    /// 光标离开事件
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        SetNormalState();
    }

    /// <summary>
    /// 设置按下状态
    /// </summary>
    void SetPressedState()
    {
        KeyImg.sprite = PressedSprite;
    }

    /// <summary>
    /// 设置正常状态
    /// </summary>
    void SetNormalState()
    {
        KeyImg.sprite = NormalSprite;
    }

    void OnDisable()
    {
        SetNormalState();
    }
}
