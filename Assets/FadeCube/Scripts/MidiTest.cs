using UnityEngine;
using UnityEditor;
using System.Collections;
using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using DjLastNight;
using System.IO;
namespace DjLastNight
{
    public class MidiTest : MonoBehaviour
    {

        public string fileName = "sixthstation";
        public GameObject cubePrefab;


        MidiTimeConverter mtc;
        List<RealNote> realNoteList;


        // Use this for initialization
        void Start()
        {
            string path = EditorUtility.OpenFilePanel("Choose a song:", "Midis/", "mid");
            //path = "Midis/" + fileName + ".mid";
            mtc = new MidiTimeConverter(path, false);
            MidiFile file = new MidiFile(path, false);
            file.Events.MidiFileType = 0;
            realNoteList = mtc.GetRealNotes(GetNoteOnList(file.Events));

            //PrintRealNotes(mtc.GetRealNotes(GetNoteOnList(file.Events)));
            //PrintEventlistContent(file.Events[0]);

            //PrintPatches(file.Events);
            StartCoroutine(MidiPlayer(file.Events));


            /**
            Debug.Log("Start");
            
            Debug.Log("File created, eventsize: " + file.Events.Tracks);

            foreach (MidiEvent note in file.Events[1])
            {

                if (note.CommandCode == MidiCommandCode.NoteOn)
                {
                    string[] list = ParseNote(note.ToString());
                    Debug.Log("Time: " + list[0]);
                }

            }
            Debug.Log("Loop done");
            **/


        }

        private void PrintRealNotes(List<RealNote> rnl)
        {
            foreach (RealNote realNote in rnl)
            {
                print(realNote.ToString());
            }
        }

        private List<MidiEvent> GetNoteOnList(MidiEventCollection mec)
        {
            List<MidiEvent> ret = new List<MidiEvent>();
            foreach (IList<MidiEvent> mel in mec)
            {
                foreach (MidiEvent mEv in mel)
                {
                    ret.Add(mEv);
                }
            }



            return ret;
        }

        IEnumerator MidiPlayer(MidiEventCollection mec)
        {
            Debug.Log("Player");
            int counter = 0;
            IList<MidiEvent> list = mec[0]; //We only have 1 track

            using (MidiOut midiOut = new MidiOut(0))
            {
                while (counter < list.Count)
                {

                    MidiEvent mEv = list[counter];

                    if (realNoteList[counter].RealTime > Time.time) //Not time yet to play TODO Make it general, not from start of app (Time.time)
                    {
                        //Debug.Log("Yield: " + list[i].AbsoluteTime + " " + Time.time);
                        yield return null;
                    }
                    else
                    {
                        //midiOut.Volume = 65535;
                        //Debug.Log(mEv.ToString());

                        bool off = false;
                        if (mEv is NoteEvent)
                            if ((mEv as NoteEvent).Velocity == 0)
                                off = true;

                        if (mEv.CommandCode == MidiCommandCode.NoteOn && !off)
                        {

                            NoteOnEvent nOE = (NoteOnEvent)mEv;
                            int note = nOE.NoteNumber;
                            int velocity = nOE.Velocity;
                            if (velocity == 0)
                            {
                                off = true;
                                continue;
                            }

                            int channel = nOE.Channel;
                            midiOut.Send(MidiMessage.StartNote(note, velocity, channel).RawData);

                            float lifeTime = (float)nOE.NoteLength / 1000;
                            lifeTime *= realNoteList[counter].Factor;
                            int relativePos = note - 40;
                            GameObject cubeCopy = Instantiate(cubePrefab, new Vector3(relativePos, channel * 3 - 20, 50), Quaternion.identity) as GameObject;
                            cubeCopy.transform.localScale *= ((float)velocity / 127.0f);
                            StartCoroutine(CubeFade(cubeCopy, lifeTime));


                        }
                        else if (mEv.CommandCode == MidiCommandCode.NoteOff || off)
                        {
                            NoteEvent nE = (NoteEvent)mEv;
                            int note = nE.NoteNumber;
                            int velocity = nE.Velocity;
                            int channel = nE.Channel;
                            midiOut.Send(MidiMessage.StopNote(note, velocity, channel).RawData);

                        }
                        else if (mEv.CommandCode == MidiCommandCode.PatchChange)
                        {
                            PatchChangeEvent pce = (PatchChangeEvent)mEv;
                            midiOut.Send(MidiMessage.ChangePatch(pce.Patch, pce.Channel).RawData);

                        }
                        else if (mEv.CommandCode == MidiCommandCode.ControlChange)
                        {
                            ControlChangeEvent cce = (ControlChangeEvent)mEv;
                            midiOut.Send(MidiMessage.ChangeControl(cce.Controller.GetHashCode(), cce.ControllerValue, cce.Channel).RawData);

                        }
                        /**
                        else if (mEv.CommandCode == MidiCommandCode.MetaEvent)
                        {

                            MetaEvent metaEv = (MetaEvent)mEv;
                            switch (metaEv.MetaEventType)
                            {
                                case MetaEventType.TrackSequenceNumber:
                                    break;
                                case MetaEventType.TextEvent:
                                    break;
                                case MetaEventType.Copyright:
                                    break;
                                case MetaEventType.SequenceTrackName:
                                    break;
                                case MetaEventType.TrackInstrumentName:
                                    break;
                                case MetaEventType.Lyric:
                                    break;
                                case MetaEventType.Marker:
                                    break;
                                case MetaEventType.CuePoint:
                                    break;
                                case MetaEventType.ProgramName:
                                    break;
                                case MetaEventType.DeviceName:
                                    break;
                                case MetaEventType.MidiChannel:
                                    break;
                                case MetaEventType.MidiPort:
                                    break;
                                case MetaEventType.EndTrack:
                                    break;
                                case MetaEventType.SetTempo:
                                    break;
                                case MetaEventType.SmpteOffset:
                                    break;
                                case MetaEventType.TimeSignature:
                                    break;
                                case MetaEventType.KeySignature:
                                    break;
                                case MetaEventType.SequencerSpecific:
                                    break;
                                default:
                                    break;
                            }


                        }**/


                        //Debug.Log("Yield: " + ((float)list[i].AbsoluteTime) / 1000 + " " + Time.time);

                        counter++;
                    }

                }
            }



        }

        IEnumerator CubeFade(GameObject copy, float lifeTime)
        {
            if (lifeTime == 0)
                print("Lifetime 0");
            float numOfParts = lifeTime / (Time.fixedDeltaTime);

            Vector3 ogSize = copy.transform.localScale;
            for (int i = 0; i < numOfParts; i++)
            {
                copy.transform.localScale = ogSize - ((ogSize / numOfParts) * i);
                yield return null;
            }
            GameObject.Destroy(copy);

        }

        void PrintEventlistContent(IList<MidiEvent> list)
        {
            foreach (MidiEvent note in list)
            {
                print("CommandCode: " + note.CommandCode.ToString() + " " + note.ToString());
            }
        }

        private void PrintPatches(MidiEventCollection events)
        {
            foreach (IList<MidiEvent> list in events)
            {
                foreach (MidiEvent note in list)
                {
                    if (note.CommandCode == MidiCommandCode.PatchChange)
                        Debug.Log(note.ToString());
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}