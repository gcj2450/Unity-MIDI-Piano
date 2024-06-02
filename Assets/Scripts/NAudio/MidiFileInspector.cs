﻿using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NAudio.Midi;

public class MidiFileInspector
{
    public MidiFile MidiFile;
    private List<TempoEvent> _tempoEvents = new List<TempoEvent>();

    private double Tempo = 0;
    Dictionary<int, List<MidiNote>> trackNotesDic = null;
    List<MidiNote> allNotes = null;
    int trackCount = 0;
    public MidiFileInspector(string fileName, bool strictCheck = false)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException(String.Format("Midi file not found at {0}!", fileName));

        try
        {
            MidiFile = new MidiFile(fileName, strictCheck);
            Init();
        }
        catch (FormatException formatEx)
        {
            throw formatEx;
        }
    }

    /// <summary>
    /// 每次都要先调一次这个方法
    /// </summary>
    /// <returns></returns>
    private void Init()
    {
        BuildTempoList();

        List<MidiNote> notes = new List<MidiNote>();
        trackNotesDic = new Dictionary<int, List<MidiNote>>();
        double defaultTempo = 120;
        trackCount = MidiFile.Tracks;
        for (int n = 0; n < MidiFile.Tracks; n++)
        {
            foreach (MidiEvent midiEvent in MidiFile.Events[n])
            {
                try
                {
                    var tempoEvent = (NAudio.Midi.TempoEvent)midiEvent;
                    defaultTempo = tempoEvent.Tempo;
                    Tempo = defaultTempo;
                }
                catch { }
            }
        }
        Debug.Log($"MidiFile.Tracks: {MidiFile.Tracks}");
        for (int n = 0; n < MidiFile.Tracks; n++)
        {
            List<MidiNote> tmpNotsList = new List<MidiNote>();
            foreach (MidiEvent midiEvent in MidiFile.Events[n])
            {
                if (MidiEvent.IsNoteOn(midiEvent))
                {
                    try
                    {
                        var t_note = (NoteOnEvent)midiEvent;
                        MidiNote noteOn = new MidiNote();

                        noteOn.Note = t_note.NoteName;
                        noteOn.NoteNumber = t_note.NoteNumber;
                        noteOn.EndTime = t_note.AbsoluteTime + t_note.NoteLength;
                        //noteOn.Channel = t_note.Channel;
                        noteOn.Channel = n;
                        noteOn.Velocity = t_note.Velocity;
                        noteOn.StartTime = t_note.AbsoluteTime;

                        if (_tempoEvents.Count > 0)
                        {
                            try
                            {
                                noteOn.Tempo = MidiFile.DeltaTicksPerQuarterNote * _tempoEvents.Last(a => a.AbsoluteTime <= t_note.AbsoluteTime).BPM / 60;
                            }
                            catch (Exception e)
                            {
                                noteOn.Tempo = MidiFile.DeltaTicksPerQuarterNote * defaultTempo / 60;
                            }
                        }
                        else
                        {
                            noteOn.Tempo = MidiFile.DeltaTicksPerQuarterNote * defaultTempo / 60;
                        }

                        noteOn.Length = t_note.NoteLength / (float)noteOn.Tempo;

                        notes.Add(noteOn);
                        tmpNotsList.Add(noteOn);
                    }
                    catch (Exception formatEx)
                    {
                        throw formatEx;
                    }
                }
            }
            Debug.Log($"Track: {n}, notesCount: {tmpNotsList.Count}");
            trackNotesDic.Add(n, SortNotes(tmpNotsList));

        }
        allNotes = SortNotes(notes);
    }

    /// <summary>
    /// 获取Tempo
    /// </summary>
    /// <returns></returns>
    public double GetTempo()
    {
        if (Tempo == 0)
        {
            Init();
        }
        return Tempo;

        //原获取Tempo方法
        //if (Tempo != 0)
        //{
        //    return Tempo;
        //}
        //else
        //{
        //    BuildTempoList();

        //    for (int n = 0; n < MidiFile.Tracks; n++)
        //    {
        //        foreach (MidiEvent midiEvent in MidiFile.Events[n])
        //        {
        //            try
        //            {
        //                var tempoEvent = (NAudio.Midi.TempoEvent)midiEvent;
        //                Tempo = tempoEvent.Tempo;
        //            }
        //            catch { }
        //        }
        //    }
        //    return Tempo;
        //}


    }

    /// <summary>
    /// 获取所有Notes
    /// </summary>
    /// <returns></returns>
    public MidiNote[] GetNotes()
    {
        if (allNotes == null)
        {
            Init();
        }
        return allNotes.ToArray();
    }
    /// <summary>
    /// 根据轨道Id，获取MidiNotes，如果超过midi轨道数，将返回第一条轨道
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public MidiNote[] GetNotesByTrackId(int _id)
    {
        if (_id > trackCount - 1)
        {
            _id=0;
        }
        if (trackNotesDic == null)
        {
            Init();
        }
        return trackNotesDic[_id].ToArray();
    }

    /// <summary>
    /// 按时间排序
    /// </summary>
    /// <param name="midiNotes"></param>
    /// <returns></returns>
    public List<MidiNote> SortNotes(List<MidiNote> midiNotes)
    {
        while (true)
        {
            bool sorted = false;

            for (int i = 0; i < midiNotes.Count - 1; i++)
            {
                if (midiNotes[i].StartTime > midiNotes[i + 1].StartTime)
                {
                    var x = midiNotes[i];
                    var y = midiNotes[i + 1];

                    midiNotes[i + 1] = x;
                    midiNotes[i] = y;

                    sorted = true;
                }
            }

            if (!sorted)
                break;
        }

        return midiNotes;
    }

    /// <summary>
    /// Builds the tempo list
    /// </summary>
    private void BuildTempoList()
    {
        var currentbpm = 120.00;
        var realtime = 0.0;
        var reldelta = 0;
        _tempoEvents = new List<TempoEvent>();

        foreach (var ev in MidiFile.Events[0])
        {
            reldelta += ev.DeltaTime;
            if (ev.CommandCode != MidiCommandCode.MetaEvent)
            {
                continue;
            }
            var tempo = (MetaEvent)ev;

            if (tempo.MetaEventType != MetaEventType.SetTempo)
            {
                continue;
            }
            var relativetime = (double)reldelta / MidiFile.DeltaTicksPerQuarterNote * (60000.0 / currentbpm);
            currentbpm = ((NAudio.Midi.TempoEvent)tempo).Tempo;
            realtime += relativetime;
            reldelta = 0;
            var tempo_event = new TempoEvent
            {
                AbsoluteTime = tempo.AbsoluteTime,
                RealTime = realtime,
                BPM = currentbpm
            };
            _tempoEvents.Add(tempo_event);
        }
    }

    /// <summary>
    /// Calculates the midi real time in seconds by given Absolute time.
    /// </summary>
    /// <param name="currentNoteAbsTime">The absolute note time which will be converted.</param>
    /// <returns></returns>
    private double GetRealtime(long currentNoteAbsTime)
    {
        var BPM = 120.0;   //As per the MIDI specification, until a tempo change is reached, 120BPM is assumed
        var reldelta = currentNoteAbsTime;   //The number of delta ticks between the delta time being converted and the tempo change immediately at or before it
        var time = 0.0;   //The real time position of the tempo change immediately at or before the delta time being converted
        foreach (var tempo in _tempoEvents.Where(tempo => tempo.AbsoluteTime <= currentNoteAbsTime))
        {
            BPM = tempo.BPM;
            time = tempo.RealTime;
            reldelta = currentNoteAbsTime - tempo.AbsoluteTime;
        }
        time += ((double)reldelta / MidiFile.DeltaTicksPerQuarterNote) * (60000.0 / BPM);
        return Math.Round(time / 1000.0, 5);
    }

    /// <summary>
    /// Helper class needed for storing tempo events
    /// with its Absolute time, Real time and BPM. 
    /// </summary>
    internal class TempoEvent
    {
        public long AbsoluteTime { get; set; }
        public double RealTime { get; set; }
        public double BPM { get; set; }
    }
}

[Serializable]
public class MidiNote
{
    public long StartTime;
    public int Channel;
    public string Note;
    public int NoteNumber;
    public long EndTime;

    public float Length;
    public int Velocity;
    public double Tempo;

    public MidiNote()
    {

    }

    public MidiNote(long startTime, int channel, string note, int length, int velocity)
    {
        StartTime = startTime;
        Channel = channel;
        Note = note;
        Length = length;
        Velocity = velocity;
    }
}