using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.RecordPlayer.Domain
{
    /// <summary>
    /// Holds read-only recording information after the duplicated tracks have been expanded out
    /// </summary>
    internal class ExpandedRecording : IPlayable
    {
        const int TRACKS = 22;

        private bool[][] _notes = new bool[TRACKS][];
        private int _beatCount;
        private string _title;

        /// <summary>
        /// Create an expanded recording from one with the tracks de-duplicated
        /// </summary>
        /// <param name="recording"></param>
        internal ExpandedRecording(Recording recording)
        {
            _beatCount = recording.BeatCount;
            _title = recording.Title;

            for (int i = 0; i < recording.TrackCount; i++)
            {
                if (i < 7)
                    CopyTrack(i, recording.GetTrack(i));
                if (i == 7)
                    SplitTrack(7, 8, recording.GetTrack(i));
                if (i == 8)
                    SplitTrack(9, 10, recording.GetTrack(i));
                if (i == 9)
                    SplitTrack(11, 12, recording.GetTrack(i));
                if (i == 10)
                    SplitTrack(13, 14, recording.GetTrack(i));
                if (i == 11)
                    SplitTrack(15, 16, recording.GetTrack(i));
                if (i == 12)
                    SplitTrack(17, 18, recording.GetTrack(i));
                if (i > 12)
                    CopyTrack(i + 6, recording.GetTrack(i));
            }
        }

        public int TrackCount
        {
            get { return TRACKS; }
        }
        public int BeatCount
        {
            get { return _beatCount; }
        }

        public bool HasNote(int track, int beat)
        {
            return _notes[track][beat];
        }

        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Copy a track
        /// </summary>
        /// <param name="newTrackNumber"></param>
        /// <param name="beats"></param>
        private void CopyTrack(int newTrackNumber, bool[] beats)
        {
            _notes[newTrackNumber] = new bool[_beatCount];
            for (int i=0; i < _beatCount; i++)
                _notes[newTrackNumber][i] = beats[i];
        }

        /// <summary>
        /// Alternate notes on the track between the two new ones
        /// </summary>
        /// <param name="firstTrackNumber"></param>
        /// <param name="secondTrackNumber"></param>
        /// <param name="beats"></param>
        private void SplitTrack(int firstTrackNumber, int secondTrackNumber, bool[] beats)
        {
            _notes[firstTrackNumber] = new bool[_beatCount];
            _notes[secondTrackNumber] = new bool[_beatCount];
            bool onSecondTrack = false;

            for (int i = 0; i < beats.Length; i++)
            {
                if (beats[i])
                {
                    _notes[onSecondTrack ? secondTrackNumber : firstTrackNumber][i] = true;
                    onSecondTrack = !onSecondTrack;
                }
            }
        }
    }
}
