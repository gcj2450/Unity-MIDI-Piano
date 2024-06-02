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

namespace CustomMidiReader
{
    public class MidiReader : MonoBehaviour
    {
        public JsonSong jSong;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<CustomNote> GetNotes()
        {
            string midiFilePath = Application.streamingAssetsPath + "/MIDI/midi.mid";
            ReadMidi2(midiFilePath);
            List<CustomNote> noteList = new List<CustomNote>();
            foreach (var item in jSong.Tracks)
            {
                noteList.AddRange(item.Notes);
            }
            noteList.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            return noteList;
        }

        //下面两个方案读取结果一样

        void ReadMidi(string midiFilePath)
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

        //写出的json格式和python的一样
        void ReadMidi2(string midiFilePath)
        {
            var midiFile = new MidiFile(midiFilePath, false);
            //var tracks = new List<Dictionary<string, object>>();
            CustomTrack customTrack = new CustomTrack();
            jSong = new JsonSong();
            jSong.Name = Path.GetFileNameWithoutExtension(midiFilePath);

            Debug.Log("TrackCount: " + midiFile.Events.Tracks);

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
                    notes.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
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

        // 基础时值，假设四分音符的时值为1.0
        private const float BaseDuration = 1.0f;

         void ReadJianPu()
        {
            string jianpu = "1- 2 3. 4 5 6- 7";
            List<float> durations = GetDurationsFromJianpu(jianpu);

            // 打印每个音符的时值
            for (int i = 0; i < durations.Count; i++)
            {
                Console.WriteLine($"音符 {i + 1} 的时值: {durations[i]}");
            }
        }

        public static List<float> GetDurationsFromJianpu(string jianpu)
        {
            List<float> durations = new List<float>();
            string[] notes = jianpu.Split(' ');

            foreach (var note in notes)
            {
                float duration = BaseDuration;

                // 检查是否有延长符号
                if (note.Contains('-'))
                {
                    duration *= 2;
                }
                if (note.Contains('.'))
                {
                    duration *= 1.5f;
                }

                durations.Add(duration);
            }

            return durations;
        }

    }

    public class CustomNote
    {
        [JsonProperty(Order = 2)]
        public int NoteNumber = 0;
        [JsonProperty(Order = 3)]
        public long StartTime = 0;
        [JsonProperty(Order = 1)]
        public long EndTime = 0;
        public CustomNote(int _noteNum, long _start, long _end)
        {
            NoteNumber = _noteNum;
            StartTime = _start;
            EndTime = _end;
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
}