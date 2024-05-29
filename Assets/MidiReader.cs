/****************************************************
    文件：MidiReader.cs
    作者：#CREATEAUTHOR#
    邮箱:  gaocanjun@baidu.com
    日期：#CREATETIME#
    功能：Todo
*****************************************************/
using NAudio.Midi;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using Formatting = Newtonsoft.Json.Formatting;

public class MidiReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string midiFilePath = Application.dataPath + "/midi.midi";
        ReadMidi2(midiFilePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //下面两个方案读取结果一样
    
    static void ReadMidi(string midiFilePath)
    {
        var midiFile = new MidiFile(midiFilePath, false);
        var tracks = new List<Dictionary<string, object>>();
        Dictionary<int, int> openNotes = new Dictionary<int, int>();

        foreach (var track in midiFile.Events)
        {
            var notes = new List<Dictionary<string, int>>();

            foreach (var midiEvent in track)
            {
                if (midiEvent.CommandCode == MidiCommandCode.NoteOn)
                {
                    var noteOn = (NoteOnEvent)midiEvent;
                    if (noteOn.Velocity > 0)
                    {
                        openNotes[noteOn.NoteNumber] = (int)noteOn.AbsoluteTime;
                    }
                    else
                    {
                        if (openNotes.TryGetValue(noteOn.NoteNumber, out int startTime))
                        {
                            notes.Add(new Dictionary<string, int>
                            {
                                { "notenumber", noteOn.NoteNumber },
                                { "start", startTime },
                                { "end", (int)noteOn.AbsoluteTime }
                            });
                            openNotes.Remove(noteOn.NoteNumber);
                        }
                    }
                }
                else if (midiEvent.CommandCode == MidiCommandCode.NoteOff)
                {
                    var noteOff = (NoteEvent)midiEvent;
                    if (openNotes.TryGetValue(noteOff.NoteNumber, out int startTime))
                    {
                        notes.Add(new Dictionary<string, int>
                        {
                            { "notenumber", noteOff.NoteNumber },
                            { "start", startTime },
                            { "end", (int)noteOff.AbsoluteTime }
                        });
                        openNotes.Remove(noteOff.NoteNumber);
                    }
                }
            }

            if (notes.Count > 0)
            {
                notes.Sort((a, b) => a["start"].CompareTo(b["start"]));
                tracks.Add(new Dictionary<string, object> { { "notes", notes } });
            }
        }

        var jsonSong = new Dictionary<string, object>
        {
            { "name", Path.GetFileNameWithoutExtension(midiFilePath) },
            { "tracks", tracks }
        };

        foreach (var midiEvent in midiFile.Events[0])
        {
            if (midiEvent is TempoEvent tempoEvent)
            {
                jsonSong["tempo"] = (int)tempoEvent.Tempo;
                break;
            }
        }

        string jsonOutputPath = Path.Combine("./", Path.GetFileNameWithoutExtension(midiFilePath) + ".json");
        Directory.CreateDirectory(Path.GetDirectoryName(jsonOutputPath));
        File.WriteAllText(jsonOutputPath, JsonConvert.SerializeObject(jsonSong, Newtonsoft.Json.Formatting.Indented));

        Console.WriteLine("MIDI data successfully written to " + jsonOutputPath);
    }

    static void ReadMidi2(string midiFilePath)
    {
        var midiFile = new MidiFile(midiFilePath, false);
        //var tracks = new List<Dictionary<string, object>>();
        CustomTrack customTrack = new CustomTrack();
        JsonSong jSong = new JsonSong();
        jSong.Name = Path.GetFileNameWithoutExtension(midiFilePath);
        foreach (var track in midiFile.Events)
        {
            List<CustomNote> notes = new List<CustomNote>();
            Dictionary<int, int> openNotes = new Dictionary<int, int>();

            foreach (var midiEvent in track)
            {
                if (midiEvent.CommandCode == MidiCommandCode.NoteOn)
                {
                    var noteOn = (NoteOnEvent)midiEvent;
                    if (noteOn.Velocity > 0)
                    {
                        openNotes[noteOn.NoteNumber] = (int)noteOn.AbsoluteTime;
                    }
                    else
                    {
                        if (openNotes.TryGetValue(noteOn.NoteNumber, out int startTime))
                        {
                            notes.Add(new CustomNote(noteOn.NoteNumber, startTime, (int)noteOn.AbsoluteTime));
                            //    notes.Add(new Dictionary<string, int>
                            //{
                            //    { "notenumber", noteOn.NoteNumber },
                            //    { "start", startTime },
                            //    { "end", (int)noteOn.AbsoluteTime }
                            //});
                            openNotes.Remove(noteOn.NoteNumber);
                        }
                    }
                }
                else if (midiEvent.CommandCode == MidiCommandCode.NoteOff)
                {
                    NoteEvent noteOff = (NoteEvent)midiEvent;

                    if (openNotes.TryGetValue(noteOff.NoteNumber, out int startTime))
                    {
                        notes.Add(new CustomNote(noteOff.NoteNumber, startTime, (int)noteOff.AbsoluteTime));

                        //    notes.Add(new Dictionary<string, int>
                        //{
                        //    { "notenumber", noteOff.NoteNumber },
                        //    { "start", startTime },
                        //    { "end", (int)noteOff.AbsoluteTime }
                        //});
                        openNotes.Remove(noteOff.NoteNumber);
                    }
                }
            }

            if (notes.Count > 0)
            {
                notes.Sort((a, b) => a.Start.CompareTo(b.Start));
                customTrack.Notes = notes;
                jSong.Tracks.Add(customTrack);
                //tracks.Add(new Dictionary<string, object> { { "notes", notes } });
            }
        }

        //    var jsonSong = new Dictionary<string, object>
        //{
        //    { "name", Path.GetFileNameWithoutExtension(midiFilePath) },
        //    { "tracks", tracks }
        //};

        foreach (var midiEvent in midiFile.Events[0])
        {
            if (midiEvent is TempoEvent tempoEvent)
            {
                //jsonSong["tempo"] = (int)tempoEvent.Tempo;
                jSong.Tempo = (int)tempoEvent.Tempo;
                break;
            }
        }

        string jsonOutputPath = Path.Combine("./", Path.GetFileNameWithoutExtension(midiFilePath) + ".json");
        Directory.CreateDirectory(Path.GetDirectoryName(jsonOutputPath));
        File.WriteAllText(jsonOutputPath, JsonConvert.SerializeObject(jSong, Formatting.Indented));

        Console.WriteLine("MIDI data successfully written to " + jsonOutputPath);
    }

}

public class CustomNote
{
    //[JsonProperty(Order = 2)]
    public int NoteNumber = 0;
    //[JsonProperty(Order = 3)]
    public long Start = 0;
    //[JsonProperty(Order = 1)]
    public long End = 0;

    public CustomNote(int _noteNum, long _start, long _end)
    {
        NoteNumber = _noteNum;
        Start = _start;
        End = _end;
    }
}

public class CustomTrack
{
    public List<CustomNote> Notes = new List<CustomNote>();
}

public class JsonSong
{
    public string Name = "";
    public List<CustomTrack> Tracks = new List<CustomTrack>();
    public int Tempo = 0;
}
