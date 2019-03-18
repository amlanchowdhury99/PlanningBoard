namespace PlanningBoard
{
    partial class Report
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
            this.ExportBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.proFromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.GenerateSummaryBtn = new System.Windows.Forms.Button();
            this.proToDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GenerateDetailsBtn = new System.Windows.Forms.Button();
            this.buyerComboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.styleComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.diaComboBox = new System.Windows.Forms.ComboBox();
            this.sizeComboBox = new System.Windows.Forms.ComboBox();
            this.partComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.shipFromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MachineComboBox = new System.Windows.Forms.ComboBox();
            this.MStatuscomboBox = new System.Windows.Forms.ComboBox();
            this.shipToDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.planBoardDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.planBoardDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.SlateGray;
            this.groupBox1.Controls.Add(this.ExportBtn);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.proFromDateTimePicker);
            this.groupBox1.Controls.Add(this.GenerateSummaryBtn);
            this.groupBox1.Controls.Add(this.proToDateTimePicker);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.GenerateDetailsBtn);
            this.groupBox1.Controls.Add(this.buyerComboBox);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.styleComboBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.diaComboBox);
            this.groupBox1.Controls.Add(this.sizeComboBox);
            this.groupBox1.Controls.Add(this.partComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.shipFromDateTimePicker);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.MachineComboBox);
            this.groupBox1.Controls.Add(this.MStatuscomboBox);
            this.groupBox1.Controls.Add(this.shipToDateTimePicker);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(998, 145);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Searching Criteria";
            // 
            // ExportBtn
            // 
            this.ExportBtn.BackColor = System.Drawing.Color.RosyBrown;
            this.ExportBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportBtn.Location = new System.Drawing.Point(860, 96);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.Size = new System.Drawing.Size(111, 31);
            this.ExportBtn.TabIndex = 57;
            this.ExportBtn.Text = "Export To Excel";
            this.ExportBtn.UseVisualStyleBackColor = false;
            this.ExportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(584, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 15);
            this.label10.TabIndex = 56;
            this.label10.Text = "Production Date :";
            // 
            // proFromDateTimePicker
            // 
            this.proFromDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.proFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.proFromDateTimePicker.Location = new System.Drawing.Point(765, 16);
            this.proFromDateTimePicker.Name = "proFromDateTimePicker";
            this.proFromDateTimePicker.Size = new System.Drawing.Size(86, 23);
            this.proFromDateTimePicker.TabIndex = 53;
            this.proFromDateTimePicker.Value = new System.DateTime(2019, 2, 5, 11, 53, 30, 0);
            this.proFromDateTimePicker.ValueChanged += new System.EventHandler(this.proFromDateTimePicker_ValueChanged);
            // 
            // GenerateSummaryBtn
            // 
            this.GenerateSummaryBtn.BackColor = System.Drawing.Color.RosyBrown;
            this.GenerateSummaryBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateSummaryBtn.Location = new System.Drawing.Point(743, 96);
            this.GenerateSummaryBtn.Name = "GenerateSummaryBtn";
            this.GenerateSummaryBtn.Size = new System.Drawing.Size(111, 31);
            this.GenerateSummaryBtn.TabIndex = 18;
            this.GenerateSummaryBtn.Text = "Show Summary";
            this.GenerateSummaryBtn.UseVisualStyleBackColor = false;
            this.GenerateSummaryBtn.Click += new System.EventHandler(this.GenerateSummaryBtn_Click);
            // 
            // proToDateTimePicker
            // 
            this.proToDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.proToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.proToDateTimePicker.Location = new System.Drawing.Point(884, 16);
            this.proToDateTimePicker.Name = "proToDateTimePicker";
            this.proToDateTimePicker.Size = new System.Drawing.Size(86, 23);
            this.proToDateTimePicker.TabIndex = 55;
            this.proToDateTimePicker.ValueChanged += new System.EventHandler(this.proToDateTimePicker_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(857, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 15);
            this.label7.TabIndex = 54;
            this.label7.Text = "To";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(723, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 15);
            this.label8.TabIndex = 52;
            this.label8.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(114, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 15);
            this.label3.TabIndex = 51;
            this.label3.Text = "Shipment Date :";
            // 
            // GenerateDetailsBtn
            // 
            this.GenerateDetailsBtn.BackColor = System.Drawing.Color.RosyBrown;
            this.GenerateDetailsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateDetailsBtn.Location = new System.Drawing.Point(627, 96);
            this.GenerateDetailsBtn.Name = "GenerateDetailsBtn";
            this.GenerateDetailsBtn.Size = new System.Drawing.Size(111, 31);
            this.GenerateDetailsBtn.TabIndex = 50;
            this.GenerateDetailsBtn.Text = "Show Details";
            this.GenerateDetailsBtn.UseVisualStyleBackColor = false;
            this.GenerateDetailsBtn.Click += new System.EventHandler(this.GenerateDetailsBtn_Click);
            // 
            // buyerComboBox
            // 
            this.buyerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buyerComboBox.FormattingEnabled = true;
            this.buyerComboBox.Location = new System.Drawing.Point(355, 56);
            this.buyerComboBox.Name = "buyerComboBox";
            this.buyerComboBox.Size = new System.Drawing.Size(84, 24);
            this.buyerComboBox.TabIndex = 45;
            this.buyerComboBox.SelectedIndexChanged += new System.EventHandler(this.buyerComboBox_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(311, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 16);
            this.label12.TabIndex = 40;
            this.label12.Text = "Buyer";
            // 
            // styleComboBox
            // 
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.FormattingEnabled = true;
            this.styleComboBox.Location = new System.Drawing.Point(484, 55);
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(84, 24);
            this.styleComboBox.TabIndex = 47;
            this.styleComboBox.SelectedIndexChanged += new System.EventHandler(this.styleComboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(445, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 16);
            this.label6.TabIndex = 44;
            this.label6.Text = "Style";
            // 
            // diaComboBox
            // 
            this.diaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.diaComboBox.FormattingEnabled = true;
            this.diaComboBox.Location = new System.Drawing.Point(729, 56);
            this.diaComboBox.Name = "diaComboBox";
            this.diaComboBox.Size = new System.Drawing.Size(84, 24);
            this.diaComboBox.TabIndex = 49;
            this.diaComboBox.SelectedIndexChanged += new System.EventHandler(this.diaComboBox_SelectedIndexChanged);
            // 
            // sizeComboBox
            // 
            this.sizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeComboBox.FormattingEnabled = true;
            this.sizeComboBox.Location = new System.Drawing.Point(608, 55);
            this.sizeComboBox.Name = "sizeComboBox";
            this.sizeComboBox.Size = new System.Drawing.Size(84, 24);
            this.sizeComboBox.TabIndex = 48;
            this.sizeComboBox.SelectedIndexChanged += new System.EventHandler(this.sizeComboBox_SelectedIndexChanged);
            // 
            // partComboBox
            // 
            this.partComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.partComboBox.FormattingEnabled = true;
            this.partComboBox.Location = new System.Drawing.Point(884, 56);
            this.partComboBox.Name = "partComboBox";
            this.partComboBox.Size = new System.Drawing.Size(84, 24);
            this.partComboBox.TabIndex = 46;
            this.partComboBox.SelectedIndexChanged += new System.EventHandler(this.partComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(698, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 16);
            this.label4.TabIndex = 42;
            this.label4.Text = "Dia";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(819, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 16);
            this.label9.TabIndex = 41;
            this.label9.Text = "Body Part";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(574, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 16);
            this.label5.TabIndex = 43;
            this.label5.Text = "Size";
            // 
            // shipFromDateTimePicker
            // 
            this.shipFromDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.shipFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.shipFromDateTimePicker.Location = new System.Drawing.Point(289, 16);
            this.shipFromDateTimePicker.Name = "shipFromDateTimePicker";
            this.shipFromDateTimePicker.Size = new System.Drawing.Size(86, 23);
            this.shipFromDateTimePicker.TabIndex = 12;
            this.shipFromDateTimePicker.Value = new System.DateTime(2019, 2, 5, 11, 53, 30, 0);
            this.shipFromDateTimePicker.ValueChanged += new System.EventHandler(this.shipFromDateTimePicker_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 38;
            this.label2.Text = "MC No";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(152, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 37;
            this.label1.Text = "MC Status";
            // 
            // MachineComboBox
            // 
            this.MachineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MachineComboBox.FormattingEnabled = true;
            this.MachineComboBox.Location = new System.Drawing.Point(62, 56);
            this.MachineComboBox.Name = "MachineComboBox";
            this.MachineComboBox.Size = new System.Drawing.Size(84, 24);
            this.MachineComboBox.TabIndex = 36;
            // 
            // MStatuscomboBox
            // 
            this.MStatuscomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MStatuscomboBox.FormattingEnabled = true;
            this.MStatuscomboBox.Location = new System.Drawing.Point(221, 56);
            this.MStatuscomboBox.Name = "MStatuscomboBox";
            this.MStatuscomboBox.Size = new System.Drawing.Size(84, 24);
            this.MStatuscomboBox.TabIndex = 35;
            this.MStatuscomboBox.SelectedIndexChanged += new System.EventHandler(this.MStatuscomboBox_SelectedIndexChanged);
            // 
            // shipToDateTimePicker
            // 
            this.shipToDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.shipToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.shipToDateTimePicker.Location = new System.Drawing.Point(408, 16);
            this.shipToDateTimePicker.Name = "shipToDateTimePicker";
            this.shipToDateTimePicker.Size = new System.Drawing.Size(86, 23);
            this.shipToDateTimePicker.TabIndex = 14;
            this.shipToDateTimePicker.ValueChanged += new System.EventHandler(this.shipToDateTimePicker_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(381, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(21, 15);
            this.label14.TabIndex = 13;
            this.label14.Text = "To";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(247, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 15);
            this.label15.TabIndex = 11;
            this.label15.Text = "From";
            // 
            // planBoardDataGridView
            // 
            this.planBoardDataGridView.AllowUserToAddRows = false;
            this.planBoardDataGridView.AllowUserToDeleteRows = false;
            this.planBoardDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.planBoardDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.planBoardDataGridView.ColumnHeadersVisible = false;
            this.planBoardDataGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.planBoardDataGridView.Location = new System.Drawing.Point(2, 153);
            this.planBoardDataGridView.Name = "planBoardDataGridView";
            this.planBoardDataGridView.ReadOnly = true;
            this.planBoardDataGridView.RowHeadersVisible = false;
            this.planBoardDataGridView.RowHeadersWidth = 4;
            this.planBoardDataGridView.Size = new System.Drawing.Size(998, 360);
            this.planBoardDataGridView.TabIndex = 3;
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 514);
            this.Controls.Add(this.planBoardDataGridView);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.planBoardDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.DateTimePicker shipFromDateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox MachineComboBox;
        private System.Windows.Forms.ComboBox MStatuscomboBox;
        private System.Windows.Forms.Button GenerateSummaryBtn;
        private System.Windows.Forms.DateTimePicker shipToDateTimePicker;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button GenerateDetailsBtn;
        private System.Windows.Forms.ComboBox buyerComboBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox styleComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox diaComboBox;
        private System.Windows.Forms.ComboBox sizeComboBox;
        private System.Windows.Forms.ComboBox partComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.DateTimePicker proFromDateTimePicker;
        private System.Windows.Forms.DateTimePicker proToDateTimePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView planBoardDataGridView;
        private System.Windows.Forms.Button ExportBtn;
    }
}