using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.ProBuilder;

public class SongControl : MonoBehaviour
{
    public PianoKeyController PianoKeyCtrller;

    private double tempo = 60f;

    MidiNote[] curMidiNotes;

    private GameObject note;
    private float speed;

    private Color32[] noteColors = new Color32[5] {
        new Color32(255, 191,   0, 1),
        new Color32(153, 102, 204, 1),
        new Color32(222,  49,  99, 1),
        new Color32( 80, 200, 120, 1),
        new Color32( 66, 134, 244, 1)
    };

    public float startPos = 0;

    /// <summary>
    /// 加载歌曲信息
    /// </summary>
    /// <param name="_song"></param>
    /// <param name="_speed"></param>
    public void LoadSongToGame(MidiNote[] _midiNotes, double _tempo, float _speed)
    {
        curMidiNotes = _midiNotes;
        speed = _speed;
        this.tempo = _tempo;
        Debug.Log($"Tempo: {tempo}");
        this.note = (GameObject)Resources.Load("Note", typeof(GameObject));
        //startPos = -60; //初始位置

        foreach (MidiNote n in curMidiNotes)
        {
            if (PianoKeyCtrller.PianoNotes.ContainsKey(n.Note))
            {
                float xcoord = PianoKeyCtrller.PianoNotes[n.Note].gameObject.transform.position.x;
                float ycoord = startPos + (n.StartTime / 60);
                float zcoord = PianoKeyCtrller.PianoNotes[n.Note].gameObject.transform.position.z;
                float len = (n.EndTime - n.StartTime) / 60;

                if (len < 50 && len > 0)
                {
                    note.GetComponent<Transform>().localScale = new Vector3(0.01f, len, 0.05f);

                    GameObject instance = Instantiate(
                        note,
                        new Vector3(xcoord, ycoord, zcoord - 0.2f),
                        new Quaternion(0f, 0f, 0f, 0f)
                    );

                    //还有一种写法tempo=PianoKeyCtrller.MidiPlayer.GlobalSpeed * speed
                    instance.GetComponent<NoteModel>().
                        SetNoteNumber(PianoKeyCtrller, n, Time.deltaTime * 4.06f * (float)tempo);
                    
                }
            }

        }

    }


}
