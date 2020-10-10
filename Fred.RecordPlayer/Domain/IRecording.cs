using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.RecordPlayer.Domain
{
    interface IPlayable
    {
        int TrackCount { get; }
        int BeatCount { get; }
        bool HasNote(int track, int beat);
        string Title { get; }
    }
}
