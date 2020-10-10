using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Fred.RecordPlayer.Domain;

namespace Fred.RecordPlayer.Controls
{
    class GridControl : Control
    {
 
        public Recording DisplayRecording { get; set; }

        const int NOTE_WIDTH = 11;
        const int NOTE_HEIGHT = 11;

        const int NOTE_SPACING = 15;
        const int TRACK_HEIGHT = 6;
        const int H_OFFSET = (NOTE_SPACING - NOTE_WIDTH) / 2;
        int LEFT_MARGIN = 3;

        const int START_MIDI_NOTE = 98;
        const int END_MIDI_NOTE = 67;
        const int ADD_ROW = 22;
        const int DELETE_ROW = 25;

        private int[] trackCentre = new int[] {
            19,16,15,14,12,11,10,
            9,8,7,6,5,4,
            //9,9,8,8,7,7,6,6,5,5,4,4,
            3,2,1
        };

        private int[] staves = new int[] { 6, 8, 10, 12, 14 };

        private SolidBrush onBrush = new SolidBrush(Color.Black);
        private Pen addPen = Pens.Blue;
        private Pen removePen = Pens.Red;

        public GridControl()
        {
            Height =  27 * TRACK_HEIGHT;
 
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DisplayRecording == null)
                return;

            Graphics g = e.Graphics;

            // Staves
            int y;
            for (int i = 0; i < staves.Length; i++)
            {
                y = staves[i] * TRACK_HEIGHT + TRACK_HEIGHT / 2;
                g.DrawLine(Pens.Black, 0, y, Width, y);
 
            }

            // Vertical Grid
            int x;
            for (int b=0; b < DisplayRecording.BeatCount; b++)
            {
                x = (b + LEFT_MARGIN) * NOTE_SPACING + NOTE_SPACING / 2;
                g.DrawLine(Pens.LightGray, x, 0 , x, Height);

                DrawDeleteAndAdd(g, b);
            }
            x = (LEFT_MARGIN - 1) * NOTE_SPACING + NOTE_SPACING / 2;
            g.DrawLine(Pens.Black, x, 0, x, Height);


            for (int track = 0; track < DisplayRecording.TrackCount; track++)
            {
                // Possible notes
                DrawNote(g, track, track % 2 - 3);

                DrawTrack(g, DisplayRecording.GetTrack(track), track);
                //_checkboxes[track] = NotesToCheckBoxes(_recording.GetTrack(track), top);
            }
        }

        public void Resize()
        {
            Width = NOTE_SPACING * (DisplayRecording.BeatCount + 10);
        }

        private void DrawDeleteAndAdd(Graphics g, int beat)
        {
            const int LINE = 4;
            int y = ADD_ROW * TRACK_HEIGHT ;
            int x = (beat + LEFT_MARGIN) * NOTE_SPACING + NOTE_SPACING / 2;

            g.DrawLine(addPen, x - LINE, y, x+ LINE, y);
            g.DrawLine(addPen, x, y - LINE, x, y + LINE);

            y += TRACK_HEIGHT * 3;
            g.DrawLine(removePen, x - LINE, y + LINE, x + LINE, y - LINE);
            g.DrawLine(removePen, x - LINE, y - LINE, x + LINE, y + LINE);
        }

        private void DrawTrack(Graphics g, bool[] notes, int track)
        {
            for (int i=0; i < notes.Length ; i++)
            {
                if (notes[i])
                    DrawNote(g, track, i);
                //else
                //    g.DrawEllipse(Pens.LightGray, i * NOTE_SPACING + H_OFFSET, noteTop, NOTE_WIDTH, NOTE_HEIGHT);
            }   

        }

        void DrawNote(Graphics g, int track, int beat)
        {

            int x = BeatLeft(beat);
            int noteTop = trackCentre[track] * TRACK_HEIGHT - TRACK_HEIGHT / 2;
            g.FillEllipse(onBrush, x, noteTop, NOTE_WIDTH, NOTE_HEIGHT);

            //int lineY = 0;
            //// line above
            //if (track == 0)
            //    lineY = noteTop;
            //// line through
            //if (track == 1 || track == 12 || track == 14)
            //    lineY = noteTop + TRACK_HEIGHT;
            //// line below
            //if (track == 13 || track == 15)
            //    lineY = noteTop + TRACK_HEIGHT * 2;
 
            //if (lineY != 0)
            //    g.DrawLine(Pens.Black, x - 2, lineY, x + NOTE_SPACING - 2, lineY);
        }

        private int BeatLeft(int beat)
        {
            return (beat + LEFT_MARGIN) * NOTE_SPACING + H_OFFSET;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int Track = GetTrack(e.Y);
            int Beat = GetBeat(e.X);
            GridAction Action = GridAction.Click;

            //MessageBox.Show(String.Format("Click {0},{1}", Track, Note));
            if (Beat < 0) return;
            if (Track == -2) Action = GridAction.AddRow;
            if (Track == -3) Action = GridAction.DeleteRow;

            if (Track == -1) return;
            if (GridClick != null)
                GridClick(this, new GridEventArgs(Track, Beat, Action));
        }

        int GetTrack(int y)
        {
            int distance;
            int closestDistance = 100;
            int track = -1;
            for (int i = 0; i < DisplayRecording.TrackCount; i++)
            {
                distance = Math.Abs(y - trackCentre[i] * TRACK_HEIGHT);
                if (distance > TRACK_HEIGHT) continue;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    track = i;
                }
            }

            if (Math.Abs(y - ADD_ROW * TRACK_HEIGHT) < TRACK_HEIGHT) return -2;
            if (Math.Abs(y - DELETE_ROW * TRACK_HEIGHT) < TRACK_HEIGHT) return -3;

            return track;
        }

        int GetBeat(int x)
        {
            if (x > (DisplayRecording.MaxBeats + 1 + LEFT_MARGIN) * NOTE_SPACING)
                return -1;

            return (x / NOTE_SPACING - LEFT_MARGIN);
        }

        public event EventHandler<GridEventArgs> GridClick;
    }
}

