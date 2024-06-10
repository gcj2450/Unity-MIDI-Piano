using UnityEngine;
using System.Collections;
namespace DjLastNight
{
    public class RealNote
    {

        public double RealTime;
        //public int NoteNumber;
        public double BPM;
        public double OffsetFromPrevious;
        public long OldAbsoluteTime;
        public float Factor; //RealTime/(OldTime/1000)

        public override string ToString()
        {
            string ret = "RealTime: " + RealTime + " BPM: " + BPM + " OffsetFromPrevious: " + OffsetFromPrevious + " OldAbsoluteTime: " + OldAbsoluteTime;
            return ret;
        }
    }
}