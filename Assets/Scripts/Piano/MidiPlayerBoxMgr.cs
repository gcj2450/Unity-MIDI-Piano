/****************************************************
    文件：MidiPlayerBoxMgr.cs
    作者：#CREATEAUTHOR#
    邮箱:  gaocanjun@baidu.com
    日期：#CREATETIME#
    功能：Todo
*****************************************************/
using SmfLiteExtension;
using symbol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MidiPlayerBoxMgr : MonoBehaviour
{
    /// <summary>
    /// 带后缀的文件名
    /// </summary>
    public string SongFileNameWithExtension = "上春山完整版.mid";
    [HideInInspector]
    public MidiNote[] MidiNotes;

    /// <summary>
    /// 第一个Note初始位置
    /// </summary>
    public float startPos = 0;

    public PianoKeyController PianoKeyCtrller;

    MidiFileInspector _midi;
    double tempo = 0;

    private GameObject notePrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            PlayMidi();
        }
    }

    void PlayMidi()
    {
        string _path = $"{Application.streamingAssetsPath}/MIDI/{SongFileNameWithExtension}";

        _midi = new MidiFileInspector(_path);

        //MidiNotes = _midi.GetNotes();
        MidiNotes = _midi.GetNotesByTrackId(1);
        tempo = _midi.GetTempo();

        LoadSongToGame(MidiNotes);
    }

    /// <summary>
    /// 加载歌曲信息
    /// </summary>
    /// <param name="_song"></param>
    /// <param name="_speed"></param>
    public void LoadSongToGame(MidiNote[] _midiNotes)
    {
        notePrefab = (GameObject)Resources.Load("NewNote", typeof(GameObject));
        //startPos = -60; //初始位置

        foreach (MidiNote mNote in _midiNotes)
        {
            if (PianoKeyCtrller.PianoNotes.ContainsKey(mNote.Note))
            {
                float xcoord = 0;
                float ycoord = startPos + (mNote.StartTime / 60);
                float zcoord = 0;
                float len = mNote.Length; //(mNote.EndTime - mNote.StartTime) / 60;
                Debug.Log($"len: {len}");

                if (len < 50 && len > 0)
                {
                    notePrefab.GetComponent<Transform>().localScale = new Vector3(0.1f, len, 0.5f);

                    GameObject instance = Instantiate(
                        notePrefab,
                        new Vector3(xcoord, ycoord, zcoord - 0.2f),
                        new Quaternion(0f, 0f, 0f, 0f)
                    );

                    instance.GetComponent<NoteBoxModel>().SetNoteNumber(mNote, tempo);

                }
            }

        }

    }

}
