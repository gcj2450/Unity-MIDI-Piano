using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System;

public class SongManager : MonoBehaviour
{
    public MidiFile midiFile;

    // Start is called before the first frame update
    void Start()
    {
        string midiPath = Application.streamingAssetsPath + "/MIDI/midi.mid";
        ReadFromFile(midiPath);
    }

    private void ReadFromFile(string _filePath)
    {
        midiFile = MidiFile.Read(_filePath);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        //make an array of notes
        Melanchall.DryWetMidi.Interaction.Note[] array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        SetTimeStamps(array);
        setBounceLane(timeStamps);
    }

    public List<double> timeStamps = new List<double>();
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        MetricTimeSpan prevTimeStamp = TimeConverter.ConvertTo<MetricTimeSpan>(array[0].Time, midiFile.GetTempoMap());
        foreach (var note in array)
        {

            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, midiFile.GetTempoMap());
            if (prevTimeStamp != metricTimeSpan)
            {
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
            prevTimeStamp = metricTimeSpan;
        }
    }

    public float xspeed = 10;
    public float yspeed = 10;

    //音符预制体
    public GameObject rectPrefab;
    public List<GameObject> laneCoords = new List<GameObject>();
    public void setBounceLane(List<double> listTimeStamps)
    {
        double prevx = 0;
        double prevy = 0;
        double prevTimeStamp = 0;
        int counter = 0;
        float currentx = 0;
        float currenty = 0;

        foreach (var timestamp in listTimeStamps)
        {
            //upper bounce rects
            if (counter % 2 == 0)
            {


                currentx = (float)(prevx + (xspeed * (timestamp - prevTimeStamp)));
                currenty = (float)(prevy + (yspeed * (timestamp - prevTimeStamp)) + 0.01);
                GameObject newRect = Instantiate(rectPrefab, new Vector3(currentx, currenty, 0), Quaternion.identity);
                laneCoords.Add(newRect);
                prevx = (float)prevx + (xspeed * (timestamp - prevTimeStamp));
                prevy = (float)prevy + (yspeed * (timestamp - prevTimeStamp));
                prevTimeStamp = timestamp;
                counter += 1;
            }
            //lower bounce rects
            else if (counter % 2 == 1)
            {
                currentx = (float)(prevx + (xspeed * (timestamp - prevTimeStamp)));
                currenty = (float)(prevy - (yspeed * (timestamp - prevTimeStamp)) - 0.01);
                GameObject newRect = Instantiate(rectPrefab, new Vector3(currentx, currenty, 0), Quaternion.identity);
                laneCoords.Add(newRect);
                prevx = (float)prevx + (xspeed * (timestamp - prevTimeStamp));
                prevy = (float)prevy - (yspeed * (timestamp - prevTimeStamp));

                prevTimeStamp = timestamp;
                counter += 1;
            }

        }

    }

}