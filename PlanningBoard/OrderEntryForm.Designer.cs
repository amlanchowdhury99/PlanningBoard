namespace PlanningBoard
{
    partial class OrderEntryForm
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
            this.entryGroupBox = new System.Windows.Forms.GroupBox();
            this.samTextBox = new System.Windows.Forms.TextBox();
            this.effTextBox = new System.Windows.Forms.TextBox();
            this.qtyTextBox = new System.Windows.Forms.TextBox();
            this.shipDatePicker = new System.Windows.Forms.DateTimePicker();
            this.AddDia = new System.Windows.Forms.Button();
            this.AddSize = new System.Windows.Forms.Button();
            this.AddStyle = new System.Windows.Forms.Button();
            this.AddPart = new System.Windows.Forms.Button();
            this.AddBuyer = new System.Windows.Forms.Button();
            this.diaComboBox = new System.Windows.Forms.ComboBox();
            this.sizeComboBox = new System.Windows.Forms.ComboBox();
            this.styleComboBox = new System.Windows.Forms.ComboBox();
            this.partComboBox = new System.Windows.Forms.ComboBox();
            this.buyerComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveOrderInfo = new System.Windows.Forms.Button();
            this.DeleteOrderInfo = new System.Windows.Forms.Button();
            this.UpdateOrderInfo = new System.Windows.Forms.Button();
            this.orderInfoDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.orderInfoDetailsdataGridView = new System.Windows.Forms.DataGridView();
            this.SL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Buyer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Style = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Part = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Efficiency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entryGroupBox.SuspendLayout();
            this.orderInfoDetailsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orderInfoDetailsdataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // entryGroupBox
            // 
            this.entryGroupBox.Controls.Add(this.samTextBox);
            this.entryGroupBox.Controls.Add(this.effTextBox);
            this.entryGroupBox.Controls.Add(this.qtyTextBox);
            this.entryGroupBox.Controls.Add(this.shipDatePicker);
            this.entryGroupBox.Controls.Add(this.AddDia);
            this.entryGroupBox.Controls.Add(this.AddSize);
            this.entryGroupBox.Controls.Add(this.AddStyle);
            this.entryGroupBox.Controls.Add(this.AddPart);
            this.entryGroupBox.Controls.Add(this.AddBuyer);
            this.entryGroupBox.Controls.Add(this.diaComboBox);
            this.entryGroupBox.Controls.Add(this.sizeComboBox);
            this.entryGroupBox.Controls.Add(this.styleComboBox);
            this.entryGroupBox.Controls.Add(this.partComboBox);
            this.entryGroupBox.Controls.Add(this.buyerComboBox);
            this.entryGroupBox.Controls.Add(this.label11);
            this.entryGroupBox.Controls.Add(this.label8);
            this.entryGroupBox.Controls.Add(this.label7);
            this.entryGroupBox.Controls.Add(this.label6);
            this.entryGroupBox.Controls.Add(this.label5);
            this.entryGroupBox.Controls.Add(this.label4);
            this.entryGroupBox.Controls.Add(this.label3);
            this.entryGroupBox.Controls.Add(this.label2);
            this.entryGroupBox.Controls.Add(this.label1);
            this.entryGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entryGroupBox.Location = new System.Drawing.Point(28, 27);
            this.entryGroupBox.Name = "entryGroupBox";
            this.entryGroupBox.Size = new System.Drawing.Size(1214, 134);
            this.entryGroupBox.TabIndex = 0;
            this.entryGroupBox.TabStop = false;
            this.entryGroupBox.Text = "Order Info";
            // 
            // samTextBox
            // 
            this.samTextBox.Location = new System.Drawing.Point(1062, 31);
            this.samTextBox.Name = "samTextBox";
            this.samTextBox.Size = new System.Drawing.Size(146, 24);
            this.samTextBox.TabIndex = 29;
            // 
            // effTextBox
            // 
            this.effTextBox.Location = new System.Drawing.Point(827, 82);
            this.effTextBox.Name = "effTextBox";
            this.effTextBox.Size = new System.Drawing.Size(143, 24);
            this.effTextBox.TabIndex = 28;
            // 
            // qtyTextBox
            // 
            this.qtyTextBox.Location = new System.Drawing.Point(325, 83);
            this.qtyTextBox.Name = "qtyTextBox";
            this.qtyTextBox.Size = new System.Drawing.Size(141, 24);
            this.qtyTextBox.TabIndex = 27;
            // 
            // shipDatePicker
            // 
            this.shipDatePicker.CustomFormat = "dd/MM/yyyy";
            this.shipDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shipDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.shipDatePicker.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.shipDatePicker.Location = new System.Drawing.Point(575, 82);
            this.shipDatePicker.Name = "shipDatePicker";
            this.shipDatePicker.Size = new System.Drawing.Size(141, 22);
            this.shipDatePicker.TabIndex = 9;
            this.shipDatePicker.Value = new System.DateTime(2018, 1, 1, 12, 0, 0, 0);
            // 
            // AddDia
            // 
            this.AddDia.Location = new System.Drawing.Point(946, 33);
            this.AddDia.Name = "AddDia";
            this.AddDia.Size = new System.Drawing.Size(24, 27);
            this.AddDia.TabIndex = 24;
            this.AddDia.Text = ">";
            this.AddDia.UseVisualStyleBackColor = true;
            this.AddDia.Click += new System.EventHandler(this.AddDia_Click);
            // 
            // AddSize
            // 
            this.AddSize.Location = new System.Drawing.Point(692, 32);
            this.AddSize.Name = "AddSize";
            this.AddSize.Size = new System.Drawing.Size(24, 27);
            this.AddSize.TabIndex = 22;
            this.AddSize.Text = ">";
            this.AddSize.UseVisualStyleBackColor = true;
            this.AddSize.Click += new System.EventHandler(this.AddSize_Click);
            // 
            // AddStyle
            // 
            this.AddStyle.Location = new System.Drawing.Point(442, 33);
            this.AddStyle.Name = "AddStyle";
            this.AddStyle.Size = new System.Drawing.Size(24, 27);
            this.AddStyle.TabIndex = 2;
            this.AddStyle.Text = ">";
            this.AddStyle.UseVisualStyleBackColor = true;
            this.AddStyle.Click += new System.EventHandler(this.AddStyle_Click);
            // 
            // AddPart
            // 
            this.AddPart.Location = new System.Drawing.Point(201, 78);
            this.AddPart.Name = "AddPart";
            this.AddPart.Size = new System.Drawing.Size(24, 27);
            this.AddPart.TabIndex = 20;
            this.AddPart.Text = ">";
            this.AddPart.UseVisualStyleBackColor = true;
            this.AddPart.Click += new System.EventHandler(this.AddPart_Click);
            // 
            // AddBuyer
            // 
            this.AddBuyer.Location = new System.Drawing.Point(201, 32);
            this.AddBuyer.Name = "AddBuyer";
            this.AddBuyer.Size = new System.Drawing.Size(24, 27);
            this.AddBuyer.TabIndex = 1;
            this.AddBuyer.Text = ">";
            this.AddBuyer.UseVisualStyleBackColor = true;
            this.AddBuyer.Click += new System.EventHandler(this.AddBuyer_Click);
            // 
            // diaComboBox
            // 
            this.diaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.diaComboBox.FormattingEnabled = true;
            this.diaComboBox.Location = new System.Drawing.Point(829, 34);
            this.diaComboBox.Name = "diaComboBox";
            this.diaComboBox.Size = new System.Drawing.Size(121, 26);
            this.diaComboBox.TabIndex = 17;
            // 
            // sizeComboBox
            // 
            this.sizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeComboBox.FormattingEnabled = true;
            this.sizeComboBox.Location = new System.Drawing.Point(575, 33);
            this.sizeComboBox.Name = "sizeComboBox";
            this.sizeComboBox.Size = new System.Drawing.Size(121, 26);
            this.sizeComboBox.TabIndex = 15;
            // 
            // styleComboBox
            // 
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.FormattingEnabled = true;
            this.styleComboBox.Location = new System.Drawing.Point(324, 34);
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(121, 26);
            this.styleComboBox.TabIndex = 13;
            // 
            // partComboBox
            // 
            this.partComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.partComboBox.FormattingEnabled = true;
            this.partComboBox.Location = new System.Drawing.Point(83, 79);
            this.partComboBox.Name = "partComboBox";
            this.partComboBox.Size = new System.Drawing.Size(121, 26);
            this.partComboBox.TabIndex = 12;
            // 
            // buyerComboBox
            // 
            this.buyerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buyerComboBox.FormattingEnabled = true;
            this.buyerComboBox.Location = new System.Drawing.Point(83, 33);
            this.buyerComboBox.Name = "buyerComboBox";
            this.buyerComboBox.Size = new System.Drawing.Size(121, 26);
            this.buyerComboBox.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(750, 82);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 18);
            this.label11.TabIndex = 10;
            this.label11.Text = "Efficiency";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1016, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 18);
            this.label8.TabIndex = 7;
            this.label8.Text = "SAM";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(501, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "ShipDate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(265, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 18);
            this.label6.TabIndex = 5;
            this.label6.Text = "Style";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(506, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(750, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Dia";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Part";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(256, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Quantity";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buyer";
            // 
            // SaveOrderInfo
            // 
            this.SaveOrderInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveOrderInfo.Location = new System.Drawing.Point(813, 167);
            this.SaveOrderInfo.Name = "SaveOrderInfo";
            this.SaveOrderInfo.Size = new System.Drawing.Size(135, 32);
            this.SaveOrderInfo.TabIndex = 1;
            this.SaveOrderInfo.Text = "Save";
            this.SaveOrderInfo.UseVisualStyleBackColor = true;
            this.SaveOrderInfo.Click += new System.EventHandler(this.SaveOrderInfo_Click_1);
            // 
            // DeleteOrderInfo
            // 
            this.DeleteOrderInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteOrderInfo.Location = new System.Drawing.Point(1101, 167);
            this.DeleteOrderInfo.Name = "DeleteOrderInfo";
            this.DeleteOrderInfo.Size = new System.Drawing.Size(141, 32);
            this.DeleteOrderInfo.TabIndex = 3;
            this.DeleteOrderInfo.Text = "Delete";
            this.DeleteOrderInfo.UseVisualStyleBackColor = true;
            this.DeleteOrderInfo.Click += new System.EventHandler(this.DeleteOrderInfo_Click);
            // 
            // UpdateOrderInfo
            // 
            this.UpdateOrderInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateOrderInfo.Location = new System.Drawing.Point(954, 167);
            this.UpdateOrderInfo.Name = "UpdateOrderInfo";
            this.UpdateOrderInfo.Size = new System.Drawing.Size(141, 32);
            this.UpdateOrderInfo.TabIndex = 4;
            this.UpdateOrderInfo.Text = "Update";
            this.UpdateOrderInfo.UseVisualStyleBackColor = true;
            this.UpdateOrderInfo.Click += new System.EventHandler(this.UpdateOrderInfo_Click);
            // 
            // orderInfoDetailsGroupBox
            // 
            this.orderInfoDetailsGroupBox.Controls.Add(this.orderInfoDetailsdataGridView);
            this.orderInfoDetailsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderInfoDetailsGroupBox.Location = new System.Drawing.Point(28, 205);
            this.orderInfoDetailsGroupBox.Name = "orderInfoDetailsGroupBox";
            this.orderInfoDetailsGroupBox.Size = new System.Drawing.Size(1214, 326);
            this.orderInfoDetailsGroupBox.TabIndex = 5;
            this.orderInfoDetailsGroupBox.TabStop = false;
            this.orderInfoDetailsGroupBox.Text = "Order Info Details";
            // 
            // orderInfoDetailsdataGridView
            // 
            this.orderInfoDetailsdataGridView.AllowUserToAddRows = false;
            this.orderInfoDetailsdataGridView.AllowUserToDeleteRows = false;
            this.orderInfoDetailsdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.orderInfoDetailsdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SL,
            this.Buyer,
            this.Style,
            this.Size,
            this.Dia,
            this.Part,
            this.Qty,
            this.Date,
            this.SAM,
            this.Efficiency,
            this.Status});
            this.orderInfoDetailsdataGridView.Location = new System.Drawing.Point(6, 23);
            this.orderInfoDetailsdataGridView.Name = "orderInfoDetailsdataGridView";
            this.orderInfoDetailsdataGridView.ReadOnly = true;
            this.orderInfoDetailsdataGridView.Size = new System.Drawing.Size(1202, 297);
            this.orderInfoDetailsdataGridView.TabIndex = 0;
            this.orderInfoDetailsdataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.orderInfoDetailsdataGridView_CellClick);
            // 
            // SL
            // 
            this.SL.FillWeight = 90F;
            this.SL.HeaderText = "SL";
            this.SL.Name = "SL";
            this.SL.ReadOnly = true;
            this.SL.Width = 70;
            // 
            // Buyer
            // 
            this.Buyer.HeaderText = "Buyer";
            this.Buyer.Name = "Buyer";
            this.Buyer.ReadOnly = true;
            this.Buyer.Width = 150;
            // 
            // Style
            // 
            this.Style.HeaderText = "Style";
            this.Style.Name = "Style";
            this.Style.ReadOnly = true;
            // 
            // Size
            // 
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            // 
            // Dia
            // 
            this.Dia.HeaderText = "Dia";
            this.Dia.Name = "Dia";
            this.Dia.ReadOnly = true;
            // 
            // Part
            // 
            this.Part.HeaderText = "Part";
            this.Part.Name = "Part";
            this.Part.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 130;
            // 
            // SAM
            // 
            this.SAM.HeaderText = "SAM";
            this.SAM.Name = "SAM";
            this.SAM.ReadOnly = true;
            // 
            // Efficiency
            // 
            this.Efficiency.HeaderText = "Efficiency";
            this.Efficiency.Name = "Efficiency";
            this.Efficiency.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 110;
            // 
            // OrderEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 543);
            this.Controls.Add(this.orderInfoDetailsGroupBox);
            this.Controls.Add(this.UpdateOrderInfo);
            this.Controls.Add(this.DeleteOrderInfo);
            this.Controls.Add(this.SaveOrderInfo);
            this.Controls.Add(this.entryGroupBox);
            this.Name = "OrderEntryForm";
            this.Text = "OrderEntryForm";
            this.Load += new System.EventHandler(this.OrderEntryForm_Load);
            this.entryGroupBox.ResumeLayout(false);
            this.entryGroupBox.PerformLayout();
            this.orderInfoDetailsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.orderInfoDetailsdataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox entryGroupBox;
        private System.Windows.Forms.Button AddDia;
        private System.Windows.Forms.Button AddSize;
        private System.Windows.Forms.Button AddStyle;
        private System.Windows.Forms.Button AddPart;
        private System.Windows.Forms.Button AddBuyer;
        private System.Windows.Forms.ComboBox diaComboBox;
        private System.Windows.Forms.ComboBox sizeComboBox;
        private System.Windows.Forms.ComboBox styleComboBox;
        private System.Windows.Forms.ComboBox partComboBox;
        private System.Windows.Forms.ComboBox buyerComboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveOrderInfo;
        private System.Windows.Forms.Button DeleteOrderInfo;
        private System.Windows.Forms.Button UpdateOrderInfo;
        private System.Windows.Forms.GroupBox orderInfoDetailsGroupBox;
        private System.Windows.Forms.DataGridView orderInfoDetailsdataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn SL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Buyer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Style;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Part;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Efficiency;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DateTimePicker shipDatePicker;
        private System.Windows.Forms.TextBox samTextBox;
        private System.Windows.Forms.TextBox effTextBox;
        private System.Windows.Forms.TextBox qtyTextBox;
    }
}