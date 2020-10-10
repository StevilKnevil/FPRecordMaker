using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.RecordPlayer
{
    public class MidiPlayingEventArgs : EventArgs
    {
        public bool Finished;
        public bool Cancel;
    }
}
