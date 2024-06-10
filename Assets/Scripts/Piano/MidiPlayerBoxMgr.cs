/****************************************************
    文件：MidiPlayerBoxMgr.cs
    作者：#CREATEAUTHOR#
    邮箱:  gaocanjun@baidu.com
    日期：#CREATETIME#
    功能：Todo
*****************************************************/
using NAudio.Midi;
using SmfLiteExtension;
using symbol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        //string _path = $"{Application.streamingAssetsPath}/MIDI/{SongFileNameWithExtension}";
        //StartLoadMidi(_path);
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
            if (PianoKeyCtrller.PianoNotes.ContainsKey(mNote.NoteName))
            {
                float xcoord = (float)_midi.GetRealtime( mNote.StartTime);
                float ycoord = -(float)_midi.GetRealtime(mNote.EndTime);
                float zcoord = PianoKeyCtrller.GetKeyIndex(mNote.NoteName);
                float len = mNote.Length; 
                float timeL =(mNote.EndTime - mNote.StartTime) / 60;
                //len: 1.417409,time: 12, False
                //Debug.Log($"len: {len},time: {timeL}, {len==timeL}");

                if (len < 50 && len > 0)
                {
                    notePrefab.GetComponent<Transform>().localScale = new Vector3((float)len, 1, 1);

                    GameObject instance = Instantiate(
                        notePrefab,
                        new Vector3(xcoord, ycoord, zcoord ),
                        new Quaternion(0f, 0f, 0f, 0f)
                    );

                    instance.GetComponent<NoteBoxModel>().SetNoteNumber(mNote, tempo);
                    instance.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                }
            }

        }

    }


    public GameObject cubePrefab; // 立方体预制件
    /// <summary>
    /// ChatGPT给出的解析方法
    /// </summary>
    /// <param name="midiFilePath"></param>
    void StartLoadMidi(string midiFilePath)
    {
        if (File.Exists(midiFilePath))
        {
            // 读取MIDI文件
            MidiFile midiFile = new MidiFile(midiFilePath, false);

            // 获取PPQ和默认的tempo
            int ticksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
            int tempo = 500000; // 默认值为120 BPM
            Debug.Log($"midiFile.Events.Tracks: {midiFile.Events.Tracks}");
            // 遍历所有轨道和事件，查找TempoEvent
            foreach (var track in midiFile.Events)
            {
                Debug.Log($"track.Count: {track.Count}");
                foreach (var midiEvent in track)
                {
                    if (midiEvent is TempoEvent tempoEvent)
                    {
                        tempo = tempoEvent.MicrosecondsPerQuarterNote;
                        break;
                    }
                }
            }

            // 遍历所有音符事件
            foreach (var track in midiFile.Events)
            {
                foreach (var midiEvent in track)
                {
                    if (midiEvent is NoteOnEvent noteOnEvent && noteOnEvent.Velocity > 0)
                    {
                        // 计算音符的开始时间和持续时间
                        double startTime = (noteOnEvent.AbsoluteTime * tempo) / (ticksPerQuarterNote * 1000000.0);
                        double duration = (noteOnEvent.NoteLength * tempo) / (ticksPerQuarterNote * 1000000.0);
                        double endTime = duration + startTime;
                        
                        // 生成立方体
                        GameObject cube = Instantiate(cubePrefab, new Vector3((float)(startTime), -(float)endTime, PianoKeyCtrller.GetKeyIndex(noteOnEvent.NoteName)), Quaternion.identity);
                        cube.transform.localScale = new Vector3((float)duration, 1, 1);
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"MIDI file not found at {midiFilePath}");
        }
    }

}
