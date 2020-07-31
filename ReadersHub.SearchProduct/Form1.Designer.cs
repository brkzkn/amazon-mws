namespace ReadersHub.SearchProduct
{
    partial class Form1
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_apply = new System.Windows.Forms.Button();
            this.lbl_log = new System.Windows.Forms.Label();
            this.rtb_log = new System.Windows.Forms.RichTextBox();
            this.btn_openFile = new System.Windows.Forms.Button();
            this.tb_fileName = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_us = new System.Windows.Forms.RadioButton();
            this.rb_uk = new System.Windows.Forms.RadioButton();
            this.pb_status = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rb_ca = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(14, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(506, 3);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(11, 91);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(506, 43);
            this.btn_apply.TabIndex = 11;
            this.btn_apply.Text = "UYGULA";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // lbl_log
            // 
            this.lbl_log.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbl_log.AutoSize = true;
            this.lbl_log.Location = new System.Drawing.Point(12, 188);
            this.lbl_log.Name = "lbl_log";
            this.lbl_log.Size = new System.Drawing.Size(40, 17);
            this.lbl_log.TabIndex = 10;
            this.lbl_log.Text = "İşlem";
            // 
            // rtb_log
            // 
            this.rtb_log.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.rtb_log.AutoWordSelection = true;
            this.rtb_log.Location = new System.Drawing.Point(12, 208);
            this.rtb_log.Name = "rtb_log";
            this.rtb_log.Size = new System.Drawing.Size(505, 296);
            this.rtb_log.TabIndex = 9;
            this.rtb_log.Text = "";
            // 
            // btn_openFile
            // 
            this.btn_openFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_openFile.Location = new System.Drawing.Point(422, 12);
            this.btn_openFile.Name = "btn_openFile";
            this.btn_openFile.Size = new System.Drawing.Size(95, 32);
            this.btn_openFile.TabIndex = 8;
            this.btn_openFile.Text = "Dosya &Aç";
            this.btn_openFile.UseVisualStyleBackColor = true;
            this.btn_openFile.Click += new System.EventHandler(this.btn_openFile_Click);
            // 
            // tb_fileName
            // 
            this.tb_fileName.BackColor = System.Drawing.Color.White;
            this.tb_fileName.Location = new System.Drawing.Point(11, 12);
            this.tb_fileName.Multiline = true;
            this.tb_fileName.Name = "tb_fileName";
            this.tb_fileName.ReadOnly = true;
            this.tb_fileName.Size = new System.Drawing.Size(404, 32);
            this.tb_fileName.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(11, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 3);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // rb_us
            // 
            this.rb_us.AutoSize = true;
            this.rb_us.Location = new System.Drawing.Point(57, 59);
            this.rb_us.Name = "rb_us";
            this.rb_us.Size = new System.Drawing.Size(109, 21);
            this.rb_us.TabIndex = 14;
            this.rb_us.Text = "amazon.com";
            this.rb_us.UseVisualStyleBackColor = true;
            // 
            // rb_uk
            // 
            this.rb_uk.AutoSize = true;
            this.rb_uk.Checked = true;
            this.rb_uk.Location = new System.Drawing.Point(206, 59);
            this.rb_uk.Name = "rb_uk";
            this.rb_uk.Size = new System.Drawing.Size(117, 21);
            this.rb_uk.TabIndex = 14;
            this.rb_uk.Text = "amazon.co.uk";
            this.rb_uk.UseVisualStyleBackColor = true;
            // 
            // pb_status
            // 
            this.pb_status.Location = new System.Drawing.Point(11, 149);
            this.pb_status.Name = "pb_status";
            this.pb_status.Size = new System.Drawing.Size(506, 36);
            this.pb_status.TabIndex = 15;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // rb_ca
            // 
            this.rb_ca.AutoSize = true;
            this.rb_ca.Location = new System.Drawing.Point(363, 59);
            this.rb_ca.Name = "rb_ca";
            this.rb_ca.Size = new System.Drawing.Size(98, 21);
            this.rb_ca.TabIndex = 14;
            this.rb_ca.Text = "amazon.ca";
            this.rb_ca.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 516);
            this.Controls.Add(this.pb_status);
            this.Controls.Add(this.rb_ca);
            this.Controls.Add(this.rb_uk);
            this.Controls.Add(this.rb_us);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.lbl_log);
            this.Controls.Add(this.rtb_log);
            this.Controls.Add(this.btn_openFile);
            this.Controls.Add(this.tb_fileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Search Product Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Label lbl_log;
        private System.Windows.Forms.RichTextBox rtb_log;
        private System.Windows.Forms.Button btn_openFile;
        private System.Windows.Forms.TextBox tb_fileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_us;
        private System.Windows.Forms.RadioButton rb_uk;
        private System.Windows.Forms.ProgressBar pb_status;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RadioButton rb_ca;
    }
}

