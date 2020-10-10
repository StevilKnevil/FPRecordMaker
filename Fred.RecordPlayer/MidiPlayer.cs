using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Fred.RecordPlayer.Domain;

namespace Fred.RecordPlayer
{
    public class MidiPlayer : IDisposable
    {
        protected delegate void MidiCallback(int handle, int msg, int instance, int param1, int param2);

        [DllImport("winmm.dll")]
        private static extern int midiOutOpen(ref int handle, int deviceID, MidiCallback proc, int instance, int flags);

        [DllImport("winmm.dll")]
        protected static extern int midiOutShortMsg(int handle, int message);

        [DllImport("winmm.dll")]
        private static extern int midiOutClose(ref int handle);

        private int handle = 0;
        private bool disposed;

        ~MidiPlayer()
        {
            Dispose();
        }

        public void Play(byte[] notes, byte volume)
        {

            GetHandle();

            const byte status = 0x9d; // no idea

            int r;
            for (int i = 0; i < notes.Length; i++)
            {
                r = midiOutShortMsg(handle, CreateMidiData(notes[i], volume, status));
            }
        }

        public void Play(Recording recording, int noteLength, byte volume)
        {

            for (int tick = 0; tick < recording.BeatCount; tick++)
            {
                Play(recording.getMidiNotes(tick), volume);
                if (MidiPlaying != null)
                {
                    MidiPlayingEventArgs e = new MidiPlayingEventArgs();
                    MidiPlaying(this,e);
                    if (e.Cancel)
                        return;
                }
                Thread.Sleep(noteLength);
            }
            if (MidiPlaying != null)
                MidiPlaying(this, new MidiPlayingEventArgs() { Finished = true });

        }
        private int CreateMidiData(byte note, byte volume, byte status)
        {
            int data = volume << 16;
            data += note << 8;
            data += status;

            return data;
        }

        void GetHandle()
        {
            if (handle == 0)
            {
                int result = midiOutOpen(ref handle, 0, null, 0, 0);
            }
        }

        public event EventHandler<MidiPlayingEventArgs> MidiPlaying;


        #region IDisposable Members

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (handle != 0)
                {
                    midiOutClose(ref handle);
                    handle = 0;
                }
            }
            disposed = true;
        }

        #endregion
    }
}
