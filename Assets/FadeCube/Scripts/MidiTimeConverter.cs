using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Midi;

namespace DjLastNight
{
    class MidiTimeConverter
    {
        private List<TempoEvent> tempoEvents;
        private MidiFile midiFile;
        private long ticksPerQuarter;
        private List<MidiEvent> absoluteNotes;

        /// <summary>
        /// Creates instance of MidiTimeConverter.
        /// </summary>
        /// <param name="pathToMidi">Absolute path to midi file including extension.</param>
        /// <param name="strictCheck">Pass true if you want to enable strict midi check.</param>
        /// <exception cref="FormatException"
        public MidiTimeConverter(string pathToMidi, bool strictCheck)
        {
            if (!File.Exists(pathToMidi))
                throw new FileNotFoundException(String.Format("Midi file not found at {0}!", pathToMidi));

            try
            {
                midiFile = new MidiFile(pathToMidi, strictCheck);
            }
            catch (FormatException formatEx)
            {
                throw formatEx;
            }

            ticksPerQuarter = midiFile.DeltaTicksPerQuarterNote;
            BuildTempoList();
        }

        /// <summary>
        /// Converts from absolute notes timing to real notes timing
        /// </summary>
        /// <param name="absNotes">List of absolute timing notes</param>
        /// <returns>List of real timing notes.</returns>
        public List<RealNote> GetRealNotes(List<MidiEvent> absNotes)
        {
            int counter = 0;
            this.absoluteNotes = absNotes;
            double currentRealTime = 0.0;
            double lastRealTime = 0.0;
            List<RealNote> output = new List<RealNote>(absNotes.Count);
            foreach (var absNote in absNotes)
            {
                counter++;
                currentRealTime = GetRealtime(absNote.AbsoluteTime);
                output.Add(new RealNote()
                {
                    RealTime = currentRealTime,
                    BPM = tempoEvents.Last(a => a.AbsoluteTime <= absNote.AbsoluteTime).BPM,
                    OffsetFromPrevious = currentRealTime - lastRealTime,
                    OldAbsoluteTime = absNote.AbsoluteTime,
                    Factor = (float)currentRealTime/(((float)absNote.AbsoluteTime) / 1000)
                    //set another properties for your custom class
                });
                lastRealTime = currentRealTime;
            }
            return output;
        }
        /// <summary>
        /// Builds the tempo list
        /// </summary>
        private void BuildTempoList()
        {
            var currentbpm = 120.00;
            var realtime = 0.0;
            var reldelta = 0;
            tempoEvents = new List<TempoEvent>();
            foreach (var ev in midiFile.Events[0])
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
                var relativetime = (double)reldelta / ticksPerQuarter * (60000.0 / currentbpm);
                currentbpm = ((NAudio.Midi.TempoEvent)tempo).Tempo;
                realtime += relativetime;
                reldelta = 0;
                var tempo_event = new TempoEvent
                {
                    AbsoluteTime = tempo.AbsoluteTime,
                    RealTime = realtime,
                    BPM = currentbpm
                };
                tempoEvents.Add(tempo_event);
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
            foreach (var tempo in tempoEvents.Where(tempo => tempo.AbsoluteTime <= currentNoteAbsTime))
            {
                BPM = tempo.BPM;
                time = tempo.RealTime;
                reldelta = currentNoteAbsTime - tempo.AbsoluteTime;
            }
            time += ((double)reldelta / ticksPerQuarter) * (60000.0 / BPM);
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
}