using System;
using System.Collections.Generic;
using System.Text;

namespace ABC
{
    /// <summary>
    /// 谱号
    /// </summary>
    public enum Clef
    {
        /// <summary>
        /// 高音
        /// </summary>
        Treble,
        /// <summary>
        /// 低音
        /// </summary>
        Bass
    }

    public class Voice
    {
        /// <summary> Unique Identifier for the voice. </summary>
        public string identifier { get; }
        /// <summary> Friendly name for the voice.  Does not necessarily need to be unique among all voices. </summary>
        public string name { get; set; }
        public Clef clef { get; set; } = Clef.Treble;
        public List<Item> items { get; } = new List<Item>();

        /// <summary>The initial key signature for this voice.  Changes to this value will be represented with <see cref="Key"/> items.</summary>
        public KeySignature initialKey { get; set; } = KeySignature.CMajor;

        /// <summary>The initial time signature for this voice.  Changes to this value will be represented with <see cref="TimeSignature"/> items.</summary>
        public string initialTimeSignature { get; set; } = string.Empty;

        public Voice(string identifier)
        {
            this.identifier = identifier;
        }
    }
}
