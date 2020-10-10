using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fred.RecordPlayer.Domain;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Fred.RecordPlayer
{
    public partial class ScadDialog : Form
    {
        public ScadDialog()
        {
            InitializeComponent();
        }

        public Recording Recording { get; set; }
        public string FilePath { get; set; }

        private void generateButton_Click(object sender, EventArgs e)
        {

            ScadWriter scad = new ScadWriter();
            ScadWriter scad2 = new ScadWriter();
            scad.Generate(new ExpandedRecording(Recording), false);

            bool hasSecondSide = type5double.Checked;
            Recording recording2 = null;
            if (hasSecondSide)
            {
                StreamReader reader2 = File.OpenText(fileTextBox.Text);
                recording2 = new Recording(reader2);
                scad2.Generate(new ExpandedRecording(recording2), true);

            }

            StreamReader reader = File.OpenText(".\\Resources\\FisherPriceTemplate.scad");

            string outputFileName = FilePath + (hasSecondSide ? " and " + recording2.Title : "") + ".scad";
            StreamWriter outputFile;
            outputFile = File.CreateText(outputFileName);

            StringBuilder template = new StringBuilder(reader.ReadToEnd());
            template.Replace(Constants.Version, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            template.Replace(Constants.DateTime, DateTime.Now.ToString("dd MMM yyyy HH:mm"));
            template.Replace(Constants.ScadQuality, qualityCheckBox.Checked ? "" : "//");
            template.Replace(Constants.ScadQuality, qualityCheckBox.Checked ? "" : "//");
            template.Replace(Constants.ScadHasSecondSide, hasSecondSide ? "1" : "0");
            template.Replace(Constants.ScadThickness, type3.Checked ? "3" : "5");
            template.Replace(Constants.ScadReplaceMain, scad.Output + scad.TitleOutput + (hasSecondSide ? scad2.Output + scad2.TitleOutput : ""));

            outputFile.Write(template.ToString());

            outputFile.Flush();
            outputFile.Close();

            outputFile = null;

            MessageBox.Show(String.Format("Successfully created:\r\n{0}", outputFileName), "SCAD file created");

            if (openCheckBox.Checked)
                Process.Start(outputFileName);

            Close();

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void type_CheckedChanged(object sender, EventArgs e)
        {
            fileSelectButton.Enabled = type5double.Checked;
        }

        private void fileSelectButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Title = "Open File for second side";

            fileDialog.Filter = "Fisher Price record files|*.fpr";

            fileDialog.InitialDirectory = ".";

            fileTextBox.Text =  (fileDialog.ShowDialog() == DialogResult.OK ? fileDialog.FileName : "");

        }

    }
}
