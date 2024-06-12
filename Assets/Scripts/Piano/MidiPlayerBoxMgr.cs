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

public enum NoteMoveDirection
{
    Vertical,
    Horizontal,
}

public class MidiPlayerBoxMgr : MonoBehaviour
{
    /// <summary>
    /// 带后缀的文件名
    /// </summary>
    public string SongFileNameWithExtension = "上春山完整版.mid";
    [HideInInspector]
    public MidiNote[] MidiNotes;

    /// <summary>
    ///Trigger的初始位置
    /// </summary>
    public Vector3 startPos = new Vector3(0, 0, 0);
    public float MoveSpeed = 10;
    /// <summary>
    /// 钢琴播放器
    /// </summary>
    public PianoKeyController PianoKeyCtrller;

    /// <summary>
    /// 音符移动方向，如果是X方向，使用Horizontal音符，如果是Y方向，使用Vertical音符
    /// </summary>
    public NoteMoveDirection NoteMoveDir = NoteMoveDirection.Horizontal;

    MidiFileInspector _midi;
    double tempo = 0;

    private GameObject notePrefab;

    List<NoteBoxModel> NoteObjects = new List<NoteBoxModel>();

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

    public GravitySource GravitySrc;
    public GameObject SphereObj;
    void PlayMidi()
    {
        string _path = $"{Application.streamingAssetsPath}/MIDI/{SongFileNameWithExtension}";

        _midi = new MidiFileInspector(_path);

        //MidiNotes = _midi.GetNotes();
        MidiNotes = _midi.GetNotesByTrackId(1);
        tempo = _midi.GetTempo();

        LoadSongToGame(MidiNotes);
        float xOffset = NoteObjects[0].transform.localScale.x * 0.5f * 5.7518f;
        SphereObj.transform.localPosition = NoteObjects[0].transform.localPosition + new Vector3(xOffset, 5, 0);
        GravitySrc.gameObject.SetActive(true);
    }

    /// <summary>
    /// 加载歌曲信息
    /// </summary>
    /// <param name="_song"></param>
    /// <param name="_speed"></param>
    public void LoadSongToGame(MidiNote[] _midiNotes)
    {
        for (int id = 0; id < _midiNotes.Length; id++)
        {
            MidiNote mNote = _midiNotes[id];
            if (PianoKeyCtrller.PianoNotes.ContainsKey(mNote.NoteName))
            {
                GenertateNoteObject(mNote, id);
            }

        }

    }

    void GenertateNoteObject(MidiNote mNote, int _id)
    {
        float xCoord = 0;
        float yCoord = 0;
        float zCoord = 0;
        GameObject noteObj;
        float len = mNote.Length;
        Vector3 pos;
        //这里的lenR=len;
        //double lenR = _midi.GetRealtime(mNote.EndTime) - _midi.GetRealtime(mNote.StartTime);

        //Debug.Log($"NoteName: {mNote.NoteName}, NoteNumber: {mNote.NoteNumber}");
        if (len < 50 && len > 0)
        {
            switch (NoteMoveDir)
            {
                case NoteMoveDirection.Vertical:
                    notePrefab = (GameObject)Resources.Load("VerticalNote", typeof(GameObject));
                    //x轴是音符在钢琴键上的索引值
                    xCoord = PianoKeyCtrller.GetKeyIndex(mNote.NoteName);
                    yCoord = -(float)_midi.GetRealtime(mNote.StartTime) * MoveSpeed;

                    pos = startPos + new Vector3(xCoord, yCoord, zCoord);
                    noteObj = Instantiate(
                    notePrefab, pos, Quaternion.identity);

                    noteObj.transform.localScale = new Vector3(1, len, 1);
                    noteObj.GetComponent<NoteBoxModel>().SetNoteNumber(mNote, tempo, _id, OnNoteTriggerEnter);
                    noteObj.GetComponentInChildren<MeshRenderer>().material.color = new Color(xCoord / 88, 1, 0);
                    NoteObjects.Add(noteObj.GetComponent<NoteBoxModel>());
                    //if (_id==0)
                    //{
                    //    GenerateSphere(pos + new Vector3(0, 1, 0));
                    //}

                    break;
                case NoteMoveDirection.Horizontal:
                    notePrefab = (GameObject)Resources.Load("HorizontalNote", typeof(GameObject));
                    //x轴是时间戳
                    xCoord = (float)_midi.GetRealtime(mNote.StartTime) * MoveSpeed;
                    //y轴是音符在钢琴键上的索引
                    yCoord = PianoKeyCtrller.GetKeyIndex(mNote.NoteName);
                    pos = startPos + new Vector3(xCoord, yCoord, zCoord);
                    noteObj = Instantiate(
                    notePrefab, pos, Quaternion.identity);

                    noteObj.transform.localScale = new Vector3(len, 1, 1);
                    noteObj.GetComponent<NoteBoxModel>().SetNoteNumber(mNote, tempo, _id, OnNoteTriggerEnter);
                    noteObj.GetComponentInChildren<MeshRenderer>().material.color = new Color(1, yCoord / 88, 0);
                    NoteObjects.Add(noteObj.GetComponent<NoteBoxModel>());
                    //if (_id == 0)
                    //{
                    //    GenerateSphere(pos + new Vector3(0, 1, 0));
                    //}

                    break;
            }

        }

    }

    private void OnNoteTriggerEnter(NoteBoxModel model)
    {
        //if (isRoll)
        //{
        //    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
        //else
        {
            SphereObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            int nextId = model.id + 1;
            if (nextId<NoteObjects.Count-1)
            {
                //下一个音符的开始时间-当前音符的开始时间+下一个音符的时间长度一半
                float timeDuration = (float)_midi.GetRealtime(NoteObjects[nextId].curNote.StartTime) - (float)_midi.GetRealtime(model.curNote.StartTime) + NoteObjects[nextId].curNote.Length * 0.5f;
                float xOffset = NoteObjects[nextId].transform.localScale.x * 0.5f * 5.7518f;    //5.7518f是音符模型的基础长度
                Vector3 throwForce = MoveUtils.CalculateBestThrowSpeed(SphereObj.transform.position, NoteObjects[nextId].transform.position+new Vector3(xOffset,1f,0), timeDuration);
                SphereObj.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.VelocityChange);
            }
            else
            {
                float timeDuration = 2;
                float xOffset = 20;    //5.7518f是音符模型的基础长度
                Vector3 throwForce = MoveUtils.CalculateBestThrowSpeed(SphereObj.transform.position, SphereObj.transform.position + new Vector3(xOffset, 1f, 0), timeDuration);
                SphereObj.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.VelocityChange);
            }
        }
    }

    //public GameObject SpherePrefab;
    //GameObject SphereObj;
    //void GenerateSphere(Vector3 pos)
    //{
    //    SphereObj = GameObject.Instantiate(SpherePrefab);
    //    SphereObj.transform.localPosition = pos;
    //    SphereObj.transform.localRotation = Quaternion.identity;
    //}


    //public GameObject cubePrefab; // 立方体预制件
    ///// <summary>
    ///// ChatGPT给出的解析方法
    ///// </summary>
    ///// <param name="midiFilePath"></param>
    //void StartLoadMidi(string midiFilePath)
    //{
    //    if (File.Exists(midiFilePath))
    //    {
    //        // 读取MIDI文件
    //        MidiFile midiFile = new MidiFile(midiFilePath, false);

    //        // 获取PPQ和默认的tempo
    //        int ticksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
    //        int tempo = 500000; // 默认值为120 BPM
    //        Debug.Log($"midiFile.Events.Tracks: {midiFile.Events.Tracks}");
    //        // 遍历所有轨道和事件，查找TempoEvent
    //        foreach (var track in midiFile.Events)
    //        {
    //            Debug.Log($"track.Count: {track.Count}");
    //            foreach (var midiEvent in track)
    //            {
    //                if (midiEvent is TempoEvent tempoEvent)
    //                {
    //                    tempo = tempoEvent.MicrosecondsPerQuarterNote;
    //                    break;
    //                }
    //            }
    //        }

    //        // 遍历所有音符事件
    //        foreach (var track in midiFile.Events)
    //        {
    //            foreach (var midiEvent in track)
    //            {
    //                if (midiEvent is NoteOnEvent noteOnEvent && noteOnEvent.Velocity > 0)
    //                {
    //                    // 计算音符的开始时间和持续时间
    //                    double startTime = (noteOnEvent.AbsoluteTime * tempo) / (ticksPerQuarterNote * 1000000.0);
    //                    double duration = (noteOnEvent.NoteLength * tempo) / (ticksPerQuarterNote * 1000000.0);
    //                    double endTime = duration + startTime;

    //                    // 生成立方体
    //                    GameObject cube = Instantiate(cubePrefab, new Vector3((float)(startTime), -(float)endTime, PianoKeyCtrller.GetKeyIndex(noteOnEvent.NoteName)), Quaternion.identity);
    //                    cube.transform.localScale = new Vector3((float)duration, 1, 1);
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"MIDI file not found at {midiFilePath}");
    //    }
    //}

}
