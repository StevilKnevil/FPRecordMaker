using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fred.RecordPlayer.Domain
{
    public class Recording : IPlayable
    {
        const int TRACKS = 16;
        const int MAX_BEATS = 200;

        private bool[][] _notes = new bool[TRACKS][];
        public static byte[] midiValues = new byte[] {
                67,72,74,76,79,81,83,
                84,//84,
                86,//86,
                88,//88,
                89,//89,
                91,//91,
                93,//93,
                95,96,98
            };

        public string Title { get; set; }
        public string Comments { get; set; }
        public int TrackCount
        {
            get { return TRACKS; }
        }
        public int MaxBeats
        {
            get { return MAX_BEATS; }
        }

        public int BeatCount
        {
            get; set;

        }

        public Recording()
        {
            // Initialise note array
            // It could be resized dynamically, but memory is cheap and life is short!
            for (int i = 0; i < TRACKS; i++ )
                _notes[i] = new bool[MAX_BEATS];

                midiValues = new byte[] {
                67,72,74,76,79,81,83,
                84,//84,
                86,//86,
                88,//88,
                89,//89,
                91,//91,
                93,//93,
                95,96,98
            };

                BeatCount = 0;
            
        }

        public Recording(StreamReader reader)
            : this()
        {

            string line;
            for (int i = 0; i < TRACKS; i++)
            {
                line = reader.ReadLine();
                if (line.StartsWith("X"))
                    line = reader.ReadLine();

                if (i == 0)
                {
                    BeatCount = line.Length;
                }
                else
                {
                    if (line.Length != BeatCount)
                        throw new ArgumentException("Note file is not valid. Each line should be the same length");
                }
                _notes[i] = ParseLine(line);
            }
            Title = reader.ReadLine();
            Comments = reader.ReadToEnd();
        }

        public void Save(StreamWriter writer)
        {
            StringBuilder line = new StringBuilder();
            for (int i = 0; i < TRACKS; i++)
            {
                line.Clear();
                for (int b = 0; b < BeatCount; b++)
                    line.Append(_notes[i][b] ? '+' : '-');

                writer.WriteLine(line.ToString());
            }
            writer.WriteLine(Title);
            writer.WriteLine(Comments);

        }

        public static int GetMidiNote(int track)
        {
            return midiValues[track];
        }

        public bool HasNote(int track, int beat)
        {
            return _notes[track][beat];
        }


        public void ToggleNote(int track, int beat)
        {
            _notes[track][beat] = !_notes[track][beat];

            if (beat + 1 > BeatCount)
                BeatCount = beat + 1;
        }

        /// <summary>
        /// Create twice as many notes with gaps between them. Ideal if you want to have more accurate timing
        /// </summary>
        public void DoubleUp()
        {
            if (BeatCount > MAX_BEATS / 2) return;
            for (int b = BeatCount; b > 0; b--)
            {
                for (int t = 0; t < TRACKS; t++)
                {
                    _notes[t][b * 2] = _notes[t][b];
                    _notes[t][b * 2 - 1] = false;
                }
            }
            BeatCount *= 2;
        }

        public void HalfDown()
        {
            for (int t = 0; t < TRACKS; t++)
            {
                for (int b = 0; b < BeatCount / 2; b++)
                {
                    _notes[t][b] = _notes[t][b * 2] || _notes[t][b * 2 + 1];
                }

                for (int b = BeatCount / 2; b < BeatCount; b++)
                    _notes[t][b] = _notes[t][b + 1] = false;
            }
            BeatCount /= 2;
        }

        public void AddRowBefore(int beat)
        {
            if (BeatCount >= MAX_BEATS) return;

            BeatCount++;
            for (int t = 0; t < TrackCount; t++)
            {
                for (int b = BeatCount; b > beat; b--)
                {
                    _notes[t][b] = _notes[t][b - 1];
                }
                _notes[t][beat] = false;
            }
        }
        public void DeleteRow(int beat)
        {
            for (int t = 0; t < TrackCount; t++)
            {
                for (int b = beat; b < BeatCount; b++)
                {
                    _notes[t][b] = _notes[t][b + 1];
                }
                _notes[t][BeatCount] = false;
            }
            BeatCount--;

        }
        public byte[] getMidiNotes(int beat)
        {
            List<byte> midiNotes = new List<byte>();
            for (int i = 0; i < TrackCount; i++)
                if (_notes[i][beat])
                    midiNotes.Add(midiValues[i]);

            return midiNotes.ToArray();
        }
        private bool[] ParseLine(string line)
        {
            bool[] noteArray = new bool[MAX_BEATS];
            for (int i = 0; i < line.Length; i++)
                noteArray[i] = line[i] == '+';

            return noteArray;
        }

        internal bool[] GetTrack(int track)
        {
            return _notes[track];
        }
    }
}
