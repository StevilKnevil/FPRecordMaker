using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.RecordPlayer.Controls
{
    public class GridEventArgs : EventArgs
    {
        public int Track;
        public int Note;
        public GridAction Action;

        public GridEventArgs(int track, int note, GridAction action)
        {
            Track = track;
            Note = note;
            Action = action;
        }

    }

    public enum GridAction
    {
        Click,
        AddRow,
        DeleteRow
    }
}
