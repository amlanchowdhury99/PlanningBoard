namespace PlanningBoard
{
    partial class FBPlanBoardForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.daysFBTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PlanDateLabel = new System.Windows.Forms.Label();
            this.FBPlanDateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.labelHeader = new System.Windows.Forms.Label();
            this.Orderlabel = new System.Windows.Forms.Label();
            this.MachineComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.SlateGray;
            this.groupBox1.Controls.Add(this.MachineComboBox);
            this.groupBox1.Controls.Add(this.daysFBTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.PlanDateLabel);
            this.groupBox1.Controls.Add(this.FBPlanDateDateTimePicker);
            this.groupBox1.Controls.Add(this.BtnExit);
            this.groupBox1.Controls.Add(this.BtnSave);
            this.groupBox1.Controls.Add(this.labelHeader);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 113);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // daysFBTextBox
            // 
            this.daysFBTextBox.Location = new System.Drawing.Point(70, 55);
            this.daysFBTextBox.Name = "daysFBTextBox";
            this.daysFBTextBox.Size = new System.Drawing.Size(100, 23);
            this.daysFBTextBox.TabIndex = 16;
            this.daysFBTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.daysFBTextBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Day";
            // 
            // PlanDateLabel
            // 
            this.PlanDateLabel.AutoSize = true;
            this.PlanDateLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlanDateLabel.Location = new System.Drawing.Point(6, 31);
            this.PlanDateLabel.Name = "PlanDateLabel";
            this.PlanDateLabel.Size = new System.Drawing.Size(58, 16);
            this.PlanDateLabel.TabIndex = 14;
            this.PlanDateLabel.Text = "PlanDate";
            // 
            // FBPlanDateDateTimePicker
            // 
            this.FBPlanDateDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.FBPlanDateDateTimePicker.Enabled = false;
            this.FBPlanDateDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FBPlanDateDateTimePicker.Location = new System.Drawing.Point(70, 26);
            this.FBPlanDateDateTimePicker.Name = "FBPlanDateDateTimePicker";
            this.FBPlanDateDateTimePicker.Size = new System.Drawing.Size(100, 23);
            this.FBPlanDateDateTimePicker.TabIndex = 13;
            this.FBPlanDateDateTimePicker.Value = new System.DateTime(2019, 2, 5, 11, 53, 30, 0);
            this.FBPlanDateDateTimePicker.ValueChanged += new System.EventHandler(this.FBPlanDateDateTimePicker_ValueChanged);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExit.Location = new System.Drawing.Point(201, 10);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(74, 23);
            this.BtnExit.TabIndex = 6;
            this.BtnExit.Text = "Close";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSave.Location = new System.Drawing.Point(96, 84);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(74, 23);
            this.BtnSave.TabIndex = 3;
            this.BtnSave.Text = "OK";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(117, -6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(0, 23);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Orderlabel
            // 
            this.Orderlabel.AutoSize = true;
            this.Orderlabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Orderlabel.ForeColor = System.Drawing.Color.AntiqueWhite;
            this.Orderlabel.Location = new System.Drawing.Point(79, 10);
            this.Orderlabel.Name = "Orderlabel";
            this.Orderlabel.Size = new System.Drawing.Size(113, 16);
            this.Orderlabel.TabIndex = 17;
            this.Orderlabel.Text = "asdasdasdasdasd";
            this.Orderlabel.Click += new System.EventHandler(this.Orderlabel_Click);
            // 
            // MachineComboBox
            // 
            this.MachineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MachineComboBox.FormattingEnabled = true;
            this.MachineComboBox.Location = new System.Drawing.Point(71, 55);
            this.MachineComboBox.Name = "MachineComboBox";
            this.MachineComboBox.Size = new System.Drawing.Size(99, 24);
            this.MachineComboBox.TabIndex = 39;
            this.MachineComboBox.Visible = false;
            // 
            // FBPlanBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(306, 154);
            this.Controls.Add(this.Orderlabel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FBPlanBoardForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FBPlanBoardForm";
            this.Load += new System.EventHandler(this.FBPlanBoardForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox daysFBTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label PlanDateLabel;
        public System.Windows.Forms.DateTimePicker FBPlanDateDateTimePicker;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label Orderlabel;
        private System.Windows.Forms.ComboBox MachineComboBox;
    }
}