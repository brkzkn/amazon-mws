namespace ReadersHub.ProductAnalyzer
{
    partial class CriteriaSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CriteriaSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nud_feedbackCount = new System.Windows.Forms.NumericUpDown();
            this.nud_feedbackRating = new System.Windows.Forms.NumericUpDown();
            this.clb_subCondition = new System.Windows.Forms.CheckedListBox();
            this.lb_feedbackRating = new System.Windows.Forms.Label();
            this.lb_feedbackCount = new System.Windows.Forms.Label();
            this.lb_subCondition = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_feedbackCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_feedbackRating)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nud_feedbackCount);
            this.groupBox1.Controls.Add(this.nud_feedbackRating);
            this.groupBox1.Controls.Add(this.clb_subCondition);
            this.groupBox1.Controls.Add(this.lb_feedbackRating);
            this.groupBox1.Controls.Add(this.lb_feedbackCount);
            this.groupBox1.Controls.Add(this.lb_subCondition);
            this.groupBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 206);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kriterler";
            // 
            // nud_feedbackCount
            // 
            this.nud_feedbackCount.Location = new System.Drawing.Point(378, 35);
            this.nud_feedbackCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_feedbackCount.Name = "nud_feedbackCount";
            this.nud_feedbackCount.Size = new System.Drawing.Size(84, 22);
            this.nud_feedbackCount.TabIndex = 8;
            this.nud_feedbackCount.ThousandsSeparator = true;
            // 
            // nud_feedbackRating
            // 
            this.nud_feedbackRating.Location = new System.Drawing.Point(144, 35);
            this.nud_feedbackRating.Name = "nud_feedbackRating";
            this.nud_feedbackRating.Size = new System.Drawing.Size(84, 22);
            this.nud_feedbackRating.TabIndex = 8;
            this.nud_feedbackRating.ThousandsSeparator = true;
            // 
            // clb_subCondition
            // 
            this.clb_subCondition.FormattingEnabled = true;
            this.clb_subCondition.Items.AddRange(new object[] {
            "New",
            "Used - Like New",
            "Used - Very Good",
            "Used - Good",
            "Used - Acceptable",
            "Used - Unacceptable"});
            this.clb_subCondition.Location = new System.Drawing.Point(144, 70);
            this.clb_subCondition.Name = "clb_subCondition";
            this.clb_subCondition.Size = new System.Drawing.Size(318, 123);
            this.clb_subCondition.TabIndex = 7;
            // 
            // lb_feedbackRating
            // 
            this.lb_feedbackRating.AutoSize = true;
            this.lb_feedbackRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lb_feedbackRating.ForeColor = System.Drawing.Color.White;
            this.lb_feedbackRating.Location = new System.Drawing.Point(9, 35);
            this.lb_feedbackRating.Name = "lb_feedbackRating";
            this.lb_feedbackRating.Size = new System.Drawing.Size(119, 18);
            this.lb_feedbackRating.TabIndex = 0;
            this.lb_feedbackRating.Text = "Feedback Rating";
            // 
            // lb_feedbackCount
            // 
            this.lb_feedbackCount.AutoSize = true;
            this.lb_feedbackCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lb_feedbackCount.ForeColor = System.Drawing.Color.White;
            this.lb_feedbackCount.Location = new System.Drawing.Point(252, 35);
            this.lb_feedbackCount.Name = "lb_feedbackCount";
            this.lb_feedbackCount.Size = new System.Drawing.Size(117, 18);
            this.lb_feedbackCount.TabIndex = 2;
            this.lb_feedbackCount.Text = "Feedback Count";
            // 
            // lb_subCondition
            // 
            this.lb_subCondition.AutoSize = true;
            this.lb_subCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lb_subCondition.ForeColor = System.Drawing.Color.White;
            this.lb_subCondition.Location = new System.Drawing.Point(9, 70);
            this.lb_subCondition.Name = "lb_subCondition";
            this.lb_subCondition.Size = new System.Drawing.Size(101, 18);
            this.lb_subCondition.TabIndex = 6;
            this.lb_subCondition.Text = "Sub Condition";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(12, 224);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(468, 35);
            this.btn_save.TabIndex = 13;
            this.btn_save.Text = "&KAYDET";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // CriteriaSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(66)))), ((int)(((byte)(121)))));
            this.ClientSize = new System.Drawing.Size(492, 277);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CriteriaSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kriter Ayarları";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_feedbackCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_feedbackRating)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lb_feedbackRating;
        private System.Windows.Forms.Label lb_feedbackCount;
        private System.Windows.Forms.Label lb_subCondition;
        private System.Windows.Forms.NumericUpDown nud_feedbackCount;
        private System.Windows.Forms.NumericUpDown nud_feedbackRating;
        private System.Windows.Forms.CheckedListBox clb_subCondition;
        private System.Windows.Forms.Button btn_save;
    }
}