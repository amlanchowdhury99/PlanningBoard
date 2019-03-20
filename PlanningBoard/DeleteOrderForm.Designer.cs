namespace PlanningBoard
{
    partial class DeleteOrderForm
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
            this.OkBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.DeleteAll = new System.Windows.Forms.RadioButton();
            this.DeleteSingle = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // OkBtn
            // 
            this.OkBtn.BackColor = System.Drawing.Color.RosyBrown;
            this.OkBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkBtn.Location = new System.Drawing.Point(58, 90);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(86, 31);
            this.OkBtn.TabIndex = 40;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = false;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RosyBrown;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(157, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 31);
            this.button1.TabIndex = 41;
            this.button1.Text = "CANCEL";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DeleteAll
            // 
            this.DeleteAll.AutoSize = true;
            this.DeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteAll.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.DeleteAll.Location = new System.Drawing.Point(43, 20);
            this.DeleteAll.Name = "DeleteAll";
            this.DeleteAll.Size = new System.Drawing.Size(190, 19);
            this.DeleteAll.TabIndex = 42;
            this.DeleteAll.TabStop = true;
            this.DeleteAll.Text = "Delete From Whole PlanTable";
            this.DeleteAll.UseVisualStyleBackColor = true;
            this.DeleteAll.Click += new System.EventHandler(this.DeleteAll_Click);
            // 
            // DeleteSingle
            // 
            this.DeleteSingle.AutoSize = true;
            this.DeleteSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteSingle.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.DeleteSingle.Location = new System.Drawing.Point(43, 48);
            this.DeleteSingle.Name = "DeleteSingle";
            this.DeleteSingle.Size = new System.Drawing.Size(203, 19);
            this.DeleteSingle.TabIndex = 43;
            this.DeleteSingle.TabStop = true;
            this.DeleteSingle.Text = "Delete From This Particular Date";
            this.DeleteSingle.UseVisualStyleBackColor = true;
            this.DeleteSingle.Click += new System.EventHandler(this.DeleteSingle_Click);
            // 
            // DeleteOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(279, 140);
            this.Controls.Add(this.DeleteSingle);
            this.Controls.Add(this.DeleteAll);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OkBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DeleteOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeleteOrderForm";
            this.Load += new System.EventHandler(this.DeleteOrderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton DeleteAll;
        private System.Windows.Forms.RadioButton DeleteSingle;
    }
}