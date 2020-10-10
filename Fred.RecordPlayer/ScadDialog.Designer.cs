namespace Fred.RecordPlayer
{
    partial class ScadDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.type3 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.type5double = new System.Windows.Forms.RadioButton();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.type5 = new System.Windows.Forms.RadioButton();
            this.fileSelectButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.titleCheckBox = new System.Windows.Forms.CheckBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.openCheckBox = new System.Windows.Forms.CheckBox();
            this.qualityCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // type3
            // 
            this.type3.AutoSize = true;
            this.type3.Checked = true;
            this.type3.Location = new System.Drawing.Point(6, 19);
            this.type3.Name = "type3";
            this.type3.Size = new System.Drawing.Size(105, 17);
            this.type3.TabIndex = 0;
            this.type3.TabStop = true;
            this.type3.Text = "3mm single sided";
            this.type3.UseVisualStyleBackColor = true;
            this.type3.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.type5double);
            this.groupBox1.Controls.Add(this.fileTextBox);
            this.groupBox1.Controls.Add(this.type5);
            this.groupBox1.Controls.Add(this.fileSelectButton);
            this.groupBox1.Controls.Add(this.type3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(409, 127);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Disc type";
            // 
            // type5double
            // 
            this.type5double.AutoSize = true;
            this.type5double.Location = new System.Drawing.Point(7, 67);
            this.type5double.Name = "type5double";
            this.type5double.Size = new System.Drawing.Size(110, 17);
            this.type5double.TabIndex = 2;
            this.type5double.Text = "5mm double sided";
            this.type5double.UseVisualStyleBackColor = true;
            this.type5double.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // fileTextBox
            // 
            this.fileTextBox.Location = new System.Drawing.Point(123, 93);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.ReadOnly = true;
            this.fileTextBox.Size = new System.Drawing.Size(280, 20);
            this.fileTextBox.TabIndex = 5;
            // 
            // type5
            // 
            this.type5.AutoSize = true;
            this.type5.Location = new System.Drawing.Point(7, 43);
            this.type5.Name = "type5";
            this.type5.Size = new System.Drawing.Size(105, 17);
            this.type5.TabIndex = 1;
            this.type5.Text = "5mm single-sided";
            this.type5.UseVisualStyleBackColor = true;
            this.type5.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // fileSelectButton
            // 
            this.fileSelectButton.Enabled = false;
            this.fileSelectButton.Location = new System.Drawing.Point(123, 64);
            this.fileSelectButton.Name = "fileSelectButton";
            this.fileSelectButton.Size = new System.Drawing.Size(102, 23);
            this.fileSelectButton.TabIndex = 4;
            this.fileSelectButton.Text = "Second side...";
            this.fileSelectButton.UseVisualStyleBackColor = true;
            this.fileSelectButton.Click += new System.EventHandler(this.fileSelectButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // titleCheckBox
            // 
            this.titleCheckBox.AutoSize = true;
            this.titleCheckBox.Checked = true;
            this.titleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.titleCheckBox.Location = new System.Drawing.Point(15, 145);
            this.titleCheckBox.Name = "titleCheckBox";
            this.titleCheckBox.Size = new System.Drawing.Size(114, 17);
            this.titleCheckBox.TabIndex = 6;
            this.titleCheckBox.Text = "Include title names";
            this.titleCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(12, 282);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(122, 23);
            this.generateButton.TabIndex = 7;
            this.generateButton.Text = "Generate SCAD file";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // openCheckBox
            // 
            this.openCheckBox.AutoSize = true;
            this.openCheckBox.Checked = true;
            this.openCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openCheckBox.Location = new System.Drawing.Point(15, 191);
            this.openCheckBox.Name = "openCheckBox";
            this.openCheckBox.Size = new System.Drawing.Size(168, 17);
            this.openCheckBox.TabIndex = 8;
            this.openCheckBox.Text = "Open SCAD file when finished";
            this.openCheckBox.UseVisualStyleBackColor = true;
            // 
            // qualityCheckBox
            // 
            this.qualityCheckBox.AutoSize = true;
            this.qualityCheckBox.Checked = true;
            this.qualityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.qualityCheckBox.Location = new System.Drawing.Point(15, 168);
            this.qualityCheckBox.Name = "qualityCheckBox";
            this.qualityCheckBox.Size = new System.Drawing.Size(134, 17);
            this.qualityCheckBox.TabIndex = 9;
            this.qualityCheckBox.Text = "Use high quality circles";
            this.qualityCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(140, 282);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(12, 214);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(405, 62);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "Note that double sided discs and lettering significantly increase the time taken " +
    "to render in OpenSCAD.";
            // 
            // ScadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 313);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.qualityCheckBox);
            this.Controls.Add(this.openCheckBox);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.titleCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "ScadDialog";
            this.Text = "How would you like your disc?";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton type3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton type5double;
        private System.Windows.Forms.RadioButton type5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button fileSelectButton;
        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.CheckBox titleCheckBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.CheckBox openCheckBox;
        private System.Windows.Forms.CheckBox qualityCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox textBox1;

    }
}