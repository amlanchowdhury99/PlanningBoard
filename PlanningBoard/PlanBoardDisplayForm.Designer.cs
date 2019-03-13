namespace PlanningBoard
{
    partial class PlanBoardDisplayForm
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
            this.components = new System.ComponentModel.Container();
            this.planBoardDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GenerateExcelBtn = new System.Windows.Forms.Button();
            this.BtnAddPlan = new System.Windows.Forms.Button();
            this.BtnGeneratePlan = new System.Windows.Forms.Button();
            this.toDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.pBContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.forwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeWorkDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.planBoardDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pBContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // planBoardDataGridView
            // 
            this.planBoardDataGridView.AllowUserToAddRows = false;
            this.planBoardDataGridView.AllowUserToDeleteRows = false;
            this.planBoardDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.planBoardDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.planBoardDataGridView.ColumnHeadersVisible = false;
            this.planBoardDataGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.planBoardDataGridView.Location = new System.Drawing.Point(12, 121);
            this.planBoardDataGridView.Name = "planBoardDataGridView";
            this.planBoardDataGridView.ReadOnly = true;
            this.planBoardDataGridView.RowHeadersVisible = false;
            this.planBoardDataGridView.RowHeadersWidth = 4;
            this.planBoardDataGridView.Size = new System.Drawing.Size(1563, 556);
            this.planBoardDataGridView.TabIndex = 0;
            this.planBoardDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.planBoardDataGridView_CellClick);
            this.planBoardDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.planBoardDataGridView_CellDoubleClick);
            this.planBoardDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.planBoardDataGridView_CellFormatting);
            this.planBoardDataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.planBoardDataGridView_CellMouseDown);
            this.planBoardDataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.planBoardDataGridView_CellMouseUp);
            this.planBoardDataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.planBoardDataGridView_CellPainting);
            this.planBoardDataGridView.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.planBoardDataGridView_ColumnAdded);
            this.planBoardDataGridView.CurrentCellChanged += new System.EventHandler(this.planBoardDataGridView_CurrentCellChanged);
            this.planBoardDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.planBoardDataGridView_DataBindingComplete);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.SlateGray;
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.GenerateExcelBtn);
            this.groupBox1.Controls.Add(this.BtnAddPlan);
            this.groupBox1.Controls.Add(this.BtnGeneratePlan);
            this.groupBox1.Controls.Add(this.toDateTimePicker);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.fromDateTimePicker);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Tai Le", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1563, 103);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Planning Board Parameter";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.RosyBrown;
            this.textBox1.Location = new System.Drawing.Point(1277, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 27);
            this.textBox1.TabIndex = 19;
            this.textBox1.Visible = false;
            // 
            // GenerateExcelBtn
            // 
            this.GenerateExcelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateExcelBtn.Location = new System.Drawing.Point(1010, 40);
            this.GenerateExcelBtn.Name = "GenerateExcelBtn";
            this.GenerateExcelBtn.Size = new System.Drawing.Size(129, 31);
            this.GenerateExcelBtn.TabIndex = 18;
            this.GenerateExcelBtn.Text = "Export To Excel";
            this.GenerateExcelBtn.UseVisualStyleBackColor = true;
            this.GenerateExcelBtn.Click += new System.EventHandler(this.GenerateExcelBtn_Click);
            // 
            // BtnAddPlan
            // 
            this.BtnAddPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAddPlan.Location = new System.Drawing.Point(883, 40);
            this.BtnAddPlan.Name = "BtnAddPlan";
            this.BtnAddPlan.Size = new System.Drawing.Size(99, 31);
            this.BtnAddPlan.TabIndex = 16;
            this.BtnAddPlan.Text = "Add Plan";
            this.BtnAddPlan.UseVisualStyleBackColor = true;
            this.BtnAddPlan.Click += new System.EventHandler(this.BtnAddPlan_Click);
            // 
            // BtnGeneratePlan
            // 
            this.BtnGeneratePlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGeneratePlan.Location = new System.Drawing.Point(754, 40);
            this.BtnGeneratePlan.Name = "BtnGeneratePlan";
            this.BtnGeneratePlan.Size = new System.Drawing.Size(99, 31);
            this.BtnGeneratePlan.TabIndex = 15;
            this.BtnGeneratePlan.Text = "Generate";
            this.BtnGeneratePlan.UseVisualStyleBackColor = true;
            this.BtnGeneratePlan.Click += new System.EventHandler(this.BtnGeneratePlan_Click);
            // 
            // toDateTimePicker
            // 
            this.toDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.toDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDateTimePicker.Location = new System.Drawing.Point(517, 40);
            this.toDateTimePicker.Name = "toDateTimePicker";
            this.toDateTimePicker.Size = new System.Drawing.Size(204, 27);
            this.toDateTimePicker.TabIndex = 14;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(485, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(26, 18);
            this.label14.TabIndex = 13;
            this.label14.Text = "To";
            // 
            // fromDateTimePicker
            // 
            this.fromDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.fromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDateTimePicker.Location = new System.Drawing.Point(242, 40);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.Size = new System.Drawing.Size(204, 27);
            this.fromDateTimePicker.TabIndex = 12;
            this.fromDateTimePicker.Value = new System.DateTime(2019, 2, 5, 11, 53, 30, 0);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(192, 40);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 18);
            this.label15.TabIndex = 11;
            this.label15.Text = "From";
            // 
            // pBContextMenuStrip
            // 
            this.pBContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forwardToolStripMenuItem,
            this.backwardToolStripMenuItem,
            this.changeWorkDateToolStripMenuItem});
            this.pBContextMenuStrip.Name = "pBContextMenuStrip";
            this.pBContextMenuStrip.Size = new System.Drawing.Size(168, 92);
            // 
            // forwardToolStripMenuItem
            // 
            this.forwardToolStripMenuItem.Name = "forwardToolStripMenuItem";
            this.forwardToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.forwardToolStripMenuItem.Text = "Forward";
            this.forwardToolStripMenuItem.Click += new System.EventHandler(this.forwardToolStripMenuItem_Click);
            // 
            // backwardToolStripMenuItem
            // 
            this.backwardToolStripMenuItem.Name = "backwardToolStripMenuItem";
            this.backwardToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.backwardToolStripMenuItem.Text = "Backward";
            this.backwardToolStripMenuItem.Click += new System.EventHandler(this.backwardToolStripMenuItem_Click);
            // 
            // changeWorkDateToolStripMenuItem
            // 
            this.changeWorkDateToolStripMenuItem.Name = "changeWorkDateToolStripMenuItem";
            this.changeWorkDateToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.changeWorkDateToolStripMenuItem.Text = "ChangeWorkDate";
            this.changeWorkDateToolStripMenuItem.Click += new System.EventHandler(this.changeWorkDateToolStripMenuItem_Click);
            // 
            // PlanBoardDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(1587, 689);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.planBoardDataGridView);
            this.Name = "PlanBoardDisplayForm";
            this.Text = "PlanBoardDisplayForm";
            this.Load += new System.EventHandler(this.PlanBoardDisplayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.planBoardDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pBContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView planBoardDataGridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker toDateTimePicker;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.DateTimePicker fromDateTimePicker;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button BtnAddPlan;
        private System.Windows.Forms.Button BtnGeneratePlan;
        private System.Windows.Forms.ContextMenuStrip pBContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem forwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backwardToolStripMenuItem;
        private System.Windows.Forms.Button GenerateExcelBtn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem changeWorkDateToolStripMenuItem;

    }
}