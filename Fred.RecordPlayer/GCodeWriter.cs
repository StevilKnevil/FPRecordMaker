﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.RecordPlayer.Domain;
using System.Configuration;

namespace Fred.RecordPlayer
{
    internal class GCodeWriter
    {

        const double CutDepth = 1.2;
        const double CutoutStep = 1;
        const double CutoutRadius = 60.575 + CutterDiameter / 2;

        // Settings
        double CutoutDepth = 3;
        int Feedrate = 200;
        private string Prefix;
        private string Suffix;

        
        const double PinWidth = 0.8;
        const double CutterDiameter = 1;

        private double _noteAngle;

        private string _firstHalfOutput;
        private string _secondHalfOutput;
        private string _fullOutput;


        public GCodeWriter()
        {

            CutoutDepth = double.Parse(ConfigurationManager.AppSettings["gcodeCutoutDepth"]);
            Feedrate = int.Parse(ConfigurationManager.AppSettings["gcodeFeedrate"]);
            Prefix = ConfigurationManager.AppSettings["gcodePrefix"];
            Suffix = ConfigurationManager.AppSettings["gcodeSuffix"];

        }

        public void Generate(IPlayable recording)
        {
            if (recording.TrackCount != Constants.TrackRadius.Length)
                throw new ArgumentException("No of tracks do not match");

            _noteAngle = 2 * Math.PI / (double)recording.BeatCount;

            int middleNote = recording.BeatCount / 2;

            _fullOutput = Generate(recording, 0, recording.BeatCount, true);
            _firstHalfOutput = Generate(recording, 0, middleNote, false);
            _secondHalfOutput = Generate(recording, middleNote, recording.BeatCount, false, true);

        }

        private string Generate(IPlayable recording, int startNote, int endNote, bool isFull = false, bool rotate = false)
        {
            StringBuilder sb = new StringBuilder(String.Format(Prefix, Feedrate));

            // Check we're aligned OK (only for half at the moment
            //sb.AppendLine("(Checking extents)");
            //SpindleHighCommand(sb);
            //GoToCommand(sb, new Point(-CutoutRadius, CutoutRadius));
            //GoToCommand(sb, new Point(CutoutRadius, 0));
            //GoToCommand(sb, new Point(0, -HeadOffset));
            sb.AppendFormat("(Autogenerated gcode for music tracks)\r\n");

            for (int track = 0; track < recording.TrackCount; track++)
            {
                // Start track
                sb.AppendFormat("(Track {0})\r\n", track);
                SpindleHighCommand(sb);
                GoToCommand(sb, PinCoordinates(track, rotate, startNote, false));
                SpindleDownCommand(sb);

                int neighbourTrack = track + (track % 2 == 0 ? 1 : -1);

                bool hasNoteOnTrack = false;
                for (int note = startNote; note < endNote; note++)
                {

                    if (recording.HasNote(track, note))
                    {
                        sb.AppendFormat("(***{0},{1}***)\r\n", track, note);
                        if (note != startNote)
                        {
                            ArcCommand(sb, PinCoordinates(track, rotate, note, false));
                        }
                        // Move across so pin isn't jagged
                        MoveToCommand(sb, PinCoordinates(neighbourTrack, rotate, note, false));
                        // Don't slice of a neighbouring note
                        if (recording.HasNote(neighbourTrack, note))
                            SpindleUpCommand(sb);
                        ArcCommand(sb, PinCoordinates(neighbourTrack, rotate, note, true));
                        if (recording.HasNote(neighbourTrack, note))
                            SpindleDownCommand(sb);
                        // Back to correct track again
                        MoveToCommand(sb, PinCoordinates(track, rotate, note, true));
                    }

//                    //Move to next note
//                    ArcCommand(sb, PinCoordinates(track, rotate, note + 1, false));

                }
                
                // Ensure we have an arc even if we had no notes at all
                if (isFull && !hasNoteOnTrack)
                    ArcCommand(sb, PinCoordinates(track, rotate, endNote / 2, false));
                ArcCommand(sb, PinCoordinates(track, rotate, endNote, false));

            }

            sb.AppendLine("(cutout)");
            if (isFull)
            {
                SpindleHighCommand(sb);
                GoToCommand(sb, new Point(CutoutRadius, 0));
                for (double c = CutoutStep; c <= CutoutDepth; c += CutoutStep)
                {
                    // Cut full circle
                    SpindleDownCommand(sb, c);
                    ArcCommand(sb, new Point(-CutoutRadius, 0));
                    ArcCommand(sb, new Point(CutoutRadius, 0));
                }
            }
            else
            {
                for (double c = CutoutStep; c <= CutoutDepth; c += CutoutStep)
                {
                    SpindleHighCommand(sb);
                    GoToCommand(sb, new Point(CutoutRadius, 0));

                    // Cut one half
                    SpindleDownCommand(sb, c);
                    ArcCommand(sb, new Point(-CutoutRadius, 0));

                }
            }

            // return to centre
            SpindleHighCommand(sb);
            GoToCommand(sb, new Point(0, 0));

            sb.Append(Suffix);

            return sb.ToString();
        }

        public string FullOutput
        {
            get { return _fullOutput; }
        }

        public string FirstHalfOutput
        {
            get { return _firstHalfOutput; }
        }

        public string SecondHalfOutput
        {
            get { return _secondHalfOutput; }
        }

        /*
        /// <summary>
        /// Calculates the angular position of a pin
        /// </summary>
        /// <returns></returns>
        public double PinAngularPosition(int noteNumber, double angle, double headOffset)
        {
            return (double)noteNumber * angle + headOffset;
        }

        /// <summary>
        /// Calculates the angular width of a pin
        /// </summary>
        /// <param name="width"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public double PinAngularWidth(double width, double radius)
        {
            // double circumference = 2 * radius * Math.PI;
            // double angle = (width * 2 * Math.PI / circumference) ;

            // Using raidans makes this stuff easy!
            return width / radius;
        }
        */
        public Point PinCoordinates(int Track, bool rotate, int Note, bool endOfPin)
        {
            // Lookup radius and calculate angle
            double radius = Constants.TrackRadius[Track];
            double angle = Note * _noteAngle;

            // Rotate
            if (rotate)
                angle += Math.PI;

            // Add track offset
            angle -= (Constants.HeadOffset / radius);

            // Add pin width for end of pin
            if (endOfPin)
                angle += (PinWidth + CutterDiameter) / radius;

            Point p = new Point();

            // anticlockwise from LHS
            p.X = radius * Math.Cos(angle);
            p.Y = radius * Math.Sin(angle);

            return p;

        }

        /*public void GetCoordinates(double radius, double angle, out double x, out double y)
        {
            // anticlockwise from LHS
            x = radius * Math.Cos(angle);
            y = radius * Math.Sin(angle);
        }

        string SpindleDownCommand(double Depth = CutDepth)
        {
            return String.Format("G1 Z-{0:0.#####}\r\n", Depth);
        }
        */
        void SpindleDownCommand(StringBuilder sb, double Depth = CutDepth)
        {
            sb.AppendFormat("G1 Z-{0:0.###}\r\n", Depth);
        }

        void SpindleUpCommand(StringBuilder sb)
        {
            sb.AppendLine("G0 Z1");
        }

        void SpindleHighCommand(StringBuilder sb)
        {
            sb.AppendLine("G0 Z5");
        }

        void ArcCommand(StringBuilder sb, Point p, string comment = null)
        {
            sb.AppendFormat("G3 X{0:0.###} Y{1:0.###} I0.0 J0.0 {2}\r\n",
                p.X, p.Y,
                String.IsNullOrEmpty(comment) ? "" : "(" + comment + ")"
                );
        }

        void GoToCommand(StringBuilder sb, Point p)
        {
            sb.AppendFormat("G0 X{0:0.###} Y{1:0.###}\r\n", p.X, p.Y);
        }
        void MoveToCommand(StringBuilder sb, Point p)
        {
            sb.AppendFormat("G1 X{0:0.###} Y{1:0.###}\r\n", p.X, p.Y);
        }
    }
}