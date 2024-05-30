using System.Collections.Generic;
namespace SmfLite
{
    public struct Score
    {
        public int? Tempo { get; set; }

        public List<ScorePart> ScoreParts { get; set; }
    }
}