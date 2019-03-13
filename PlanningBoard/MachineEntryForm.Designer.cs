namespace PlanningBoard
{
    partial class MachineEntryForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MStatuscomboBox = new System.Windows.Forms.ComboBox();
            this.machineInfoDataGridView = new System.Windows.Forms.DataGridView();
            this.SL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MachineNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MachineDia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MachineStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaveMachineInfo = new System.Windows.Forms.Button();
            this.UpdateMachineInfo = new System.Windows.Forms.Button();
            this.MNotextBox = new System.Windows.Forms.TextBox();
            this.MDiatextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.machineInfoDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(137, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Machine No";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(137, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(137, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Machine Dia";
            // 
            // MStatuscomboBox
            // 
            this.MStatuscomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MStatuscomboBox.FormattingEnabled = true;
            this.MStatuscomboBox.Location = new System.Drawing.Point(251, 165);
            this.MStatuscomboBox.Name = "MStatuscomboBox";
            this.MStatuscomboBox.Size = new System.Drawing.Size(133, 21);
            this.MStatuscomboBox.TabIndex = 1006;
            this.MStatuscomboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MStatuscomboBox_KeyPress);
            // 
            // machineInfoDataGridView
            // 
            this.machineInfoDataGridView.AllowUserToAddRows = false;
            this.machineInfoDataGridView.AllowUserToDeleteRows = false;
            this.machineInfoDataGridView.AllowUserToOrderColumns = true;
            this.machineInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.machineInfoDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SL,
            this.MachineNumber,
            this.MachineDia,
            this.MachineStatus});
            this.machineInfoDataGridView.Location = new System.Drawing.Point(141, 239);
            this.machineInfoDataGridView.Name = "machineInfoDataGridView";
            this.machineInfoDataGridView.Size = new System.Drawing.Size(620, 248);
            this.machineInfoDataGridView.TabIndex = 1007;
            this.machineInfoDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.machineInfoDataGridView_CellClick);
            this.machineInfoDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.machineInfoDataGridView_CellContentClick);
            this.machineInfoDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.machineInfoDataGridView_CellDoubleClick);
            // 
            // SL
            // 
            this.SL.HeaderText = "SL";
            this.SL.Name = "SL";
            this.SL.Width = 90;
            // 
            // MachineNumber
            // 
            this.MachineNumber.HeaderText = "Machine Number";
            this.MachineNumber.Name = "MachineNumber";
            this.MachineNumber.Width = 200;
            // 
            // MachineDia
            // 
            this.MachineDia.HeaderText = "Machine Dia";
            this.MachineDia.Name = "MachineDia";
            this.MachineDia.Width = 150;
            // 
            // MachineStatus
            // 
            this.MachineStatus.HeaderText = "Machine Status";
            this.MachineStatus.Name = "MachineStatus";
            this.MachineStatus.Width = 135;
            // 
            // SaveMachineInfo
            // 
            this.SaveMachineInfo.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveMachineInfo.Location = new System.Drawing.Point(485, 103);
            this.SaveMachineInfo.Name = "SaveMachineInfo";
            this.SaveMachineInfo.Size = new System.Drawing.Size(134, 41);
            this.SaveMachineInfo.TabIndex = 1008;
            this.SaveMachineInfo.Text = "Save";
            this.SaveMachineInfo.UseVisualStyleBackColor = true;
            this.SaveMachineInfo.Click += new System.EventHandler(this.SaveMachineInfo_Click);
            // 
            // UpdateMachineInfo
            // 
            this.UpdateMachineInfo.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateMachineInfo.Location = new System.Drawing.Point(627, 493);
            this.UpdateMachineInfo.Name = "UpdateMachineInfo";
            this.UpdateMachineInfo.Size = new System.Drawing.Size(134, 41);
            this.UpdateMachineInfo.TabIndex = 1009;
            this.UpdateMachineInfo.Text = "Update";
            this.UpdateMachineInfo.UseVisualStyleBackColor = true;
            this.UpdateMachineInfo.Click += new System.EventHandler(this.UpdateMachineInfo_Click);
            // 
            // MNotextBox
            // 
            this.MNotextBox.Location = new System.Drawing.Point(251, 65);
            this.MNotextBox.Name = "MNotextBox";
            this.MNotextBox.Size = new System.Drawing.Size(133, 20);
            this.MNotextBox.TabIndex = 1010;
            this.MNotextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MNotextBox_KeyPress);
            // 
            // MDiatextBox
            // 
            this.MDiatextBox.Location = new System.Drawing.Point(251, 115);
            this.MDiatextBox.Name = "MDiatextBox";
            this.MDiatextBox.Size = new System.Drawing.Size(133, 20);
            this.MDiatextBox.TabIndex = 1011;
            this.MDiatextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MDiatextBox_KeyPress);
            // 
            // MachineEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(979, 564);
            this.Controls.Add(this.MDiatextBox);
            this.Controls.Add(this.MNotextBox);
            this.Controls.Add(this.UpdateMachineInfo);
            this.Controls.Add(this.SaveMachineInfo);
            this.Controls.Add(this.machineInfoDataGridView);
            this.Controls.Add(this.MStatuscomboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MachineEntryForm";
            this.Text = "MachineEntryForm";
            this.Load += new System.EventHandler(this.MachineEntryForm_Load);
            this.MouseHover += new System.EventHandler(this.MachineEntryForm_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.machineInfoDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox MStatuscomboBox;
        private System.Windows.Forms.DataGridView machineInfoDataGridView;
        private System.Windows.Forms.Button SaveMachineInfo;
        private System.Windows.Forms.Button UpdateMachineInfo;
        private System.Windows.Forms.TextBox MNotextBox;
        private System.Windows.Forms.TextBox MDiatextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn SL;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineDia;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineStatus;
    }
}