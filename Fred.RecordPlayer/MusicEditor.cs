using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Fred.RecordPlayer.Controls;
using System.Threading;
using Fred.RecordPlayer.Domain;

namespace Fred.RecordPlayer
{
    public partial class MusicEditor : Form
    {
        private Recording _recording;
        private MidiPlayer _player;

        private string recordingFileName;
        private bool fileDirty;

        bool stopPlaying;
        bool shortWarned = false;

        public MusicEditor()
        {
            InitializeComponent();

            _player = new MidiPlayer();
            _player.MidiPlaying += midi_Playing;

            scrollPanel.HorizontalScroll.Enabled = true;

        }

        private void openRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = GetInputFileName();
            if (!String.IsNullOrEmpty(fileName))
            {
                LoadFile(fileName);
           }
        }

        private string GetInputFileName()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Fisher Price Record|*.fpr";
            dialog.DefaultExt = "fpr";
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
                return dialog.FileName;
            else
                return null;

        }

        private string GetOutputFileName()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Fisher Price Record|*.fpr";
            dialog.DefaultExt = "fpr";
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
                return dialog.FileName;
            else
                return null;

        }

        private void LoadFile(string fileName)
        {
            recordingFileName = fileName;
            StreamReader reader = File.OpenText(recordingFileName);


            _recording = new Recording(reader);
            reader.Close();
            reader = null;
            grid.DisplayRecording = _recording;
            fileDirty = false;

            ShowRecording();
        }

        private void ShowRecording()
        {
            titleTextBox.Text = _recording.Title;
            commentsTextBox.Text = _recording.Comments;
            grid.Resize();
            grid.Invalidate();
            grid.Update();
         }


        private void Form1_Load(object sender, EventArgs e)
        {
            _recording = new Recording();
            grid.DisplayRecording = _recording;
            ShowRecording();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (_recording.BeatCount == 0) return;

           
            stopPlaying = false;
            int beatLength = 250;
            if (_recording.BeatCount < 25)
            {
                if (!shortWarned)
                    MessageBox.Show("Whatever you create will play in one rotation of the disc - about 25 seconds - so the speed depends on how many notes you have. As your composition is very short it will be played a bit faster.","Short composition");
                shortWarned = true;
            }
            else
            {
                // Whole track should last 25 seconds
                beatLength = 25000 / _recording.BeatCount;
            }
            Thread t = new Thread(new ThreadStart(() => _player.Play(_recording, beatLength, 60)));
            t.Start();
            playButton.Enabled = false;
            stopButton.Enabled = true;

        }


        private void grid_Click(object sender, GridEventArgs e)
        {
            switch (e.Action)
            {
                case GridAction.AddRow:
                    _recording.AddRowBefore(e.Note);
                    break;
                case GridAction.DeleteRow:
                    _recording.DeleteRow(e.Note);
                    break;
                default:
                    _recording.ToggleNote(e.Track, e.Note);
          _player.Play(new byte[] { Recording.midiValues[e.Track] }, 255);
                    break;
            }
            fileDirty = true;
            ShowRecording();
        }

        private void midi_Playing(object sender, MidiPlayingEventArgs e)
        {
            if (e.Finished)
            {
                Invoke(new Action(() =>
                {
                    playButton.Enabled = true;
                    stopButton.Enabled = false;
                }));
            }
            e.Cancel = stopPlaying;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopPlaying = true;
            playButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveRecording();
        }
        private void SaveRecording()
        {
            _recording.Title = titleTextBox.Text;
            _recording.Comments = commentsTextBox.Text;
            if (String.IsNullOrEmpty(recordingFileName))
            {
                recordingFileName = GetOutputFileName();
            }
            if (String.IsNullOrEmpty(recordingFileName))
                return;

            StreamWriter writer = File.CreateText(recordingFileName);
            _recording.Save(writer);
            writer.Flush();
            writer.Close();
            writer = null;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveIfRequired()) return;
            _recording = new Recording();
            recordingFileName = null;
            fileDirty = true;
            grid.DisplayRecording = _recording;
            ShowRecording();
        }

        private bool SaveIfRequired()
        {
            if (fileDirty)
            {
                switch(MessageBox.Show("Save this file?", "Save", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveRecording();
                        return false;
                    case DialogResult.No:
                        return false;
                    case DialogResult.Cancel:
                        return true;
                }
            }
            return false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveIfRequired())
                return;
            Application.Exit();
        }

        private void createGCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(recordingFileName) + "\\" + Path.GetFileNameWithoutExtension(recordingFileName);

            GCodeWriter gcode = new GCodeWriter();

            gcode.Generate(new ExpandedRecording(_recording));

            StreamWriter file;

            file = File.CreateText(path + " (Full).nc");
            file.Write(gcode.FullOutput);
            file.Flush();
            file.Close();

            file = File.CreateText(path + " (Part 1 of 2).nc");
            file.Write(gcode.FirstHalfOutput);
            file.Flush();
            file.Close();

            file = File.CreateText(path + " (Part 2 of 2).nc");
            file.Write(gcode.SecondHalfOutput);
            file.Flush();
            file.Close();

            file = null;

            MessageBox.Show(String.Format("Successfully created the following files:\r\n\r\n{0} (Full).nc\r\n{0} (Part 1 of 2).nc\r\n{0} (Part 1 of 2).nc\r\n", path), "Gcode files created");


        }

        private void createSTLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScadDialog d = new ScadDialog() {
                Recording = _recording,
                FilePath = Path.GetDirectoryName(recordingFileName) + "\\" + Path.GetFileNameWithoutExtension(recordingFileName)
            };

            d.ShowDialog();

            /*
            bool hasSecondSide = true;
            string secondFile = "C:\\Workspace\\Visual Studio\\Fred.RecordPlayer\\Files\\Star Wars.fpr";

            ScadWriter scad = new ScadWriter();
            ScadWriter scad2 = new ScadWriter();
            scad.Generate(new ExpandedRecording(_recording), false);

            if (hasSecondSide)
            {
                StreamReader reader2 = File.OpenText(secondFile);
                Recording recording2 = new Recording(reader2);
                scad2.Generate(new ExpandedRecording(recording2), true);

            }

            StreamReader reader = File.OpenText(".\\Resources\\FisherPriceTemplate.scad");


            string path = Path.GetDirectoryName(recordingFileName) + "\\" + Path.GetFileNameWithoutExtension(recordingFileName);
            
            StreamWriter outputFile;
            outputFile = File.CreateText(path + ".scad");

            StringBuilder template = new StringBuilder(reader.ReadToEnd());
            template.Replace(Constants.ScadHasSecondSide, hasSecondSide ? "1" : "0");
            template.Replace(Constants.ScadThickness, "5");
            template.Replace(Constants.ScadReplaceMain, scad.Output + scad.TitleOutput +  (hasSecondSide ? scad2.Output + scad2.TitleOutput : ""));

            outputFile.Write(template.ToString());

            outputFile.Flush();
            outputFile.Close();

            outputFile = null;

            MessageBox.Show(String.Format("Successfully created:\r\n{0}.scad", path), "SCAD file created");
             * */

        }

       private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void doubleTheNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _recording.DoubleUp();
            ShowRecording();
        }

        private void halfTheNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _recording.HalfDown();
            ShowRecording();
        }


    }
}
