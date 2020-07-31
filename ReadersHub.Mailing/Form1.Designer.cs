namespace ReadersHub.Mailing
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
            this.tb_fileName = new System.Windows.Forms.TextBox();
            this.btn_openFile = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.rtb_log = new System.Windows.Forms.RichTextBox();
            this.lbl_log = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_apply = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_store = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pb_status = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // tb_fileName
            // 
            this.tb_fileName.BackColor = System.Drawing.Color.White;
            this.tb_fileName.Location = new System.Drawing.Point(9, 44);
            this.tb_fileName.Margin = new System.Windows.Forms.Padding(2);
            this.tb_fileName.Multiline = true;
            this.tb_fileName.Name = "tb_fileName";
            this.tb_fileName.ReadOnly = true;
            this.tb_fileName.Size = new System.Drawing.Size(304, 27);
            this.tb_fileName.TabIndex = 0;
            // 
            // btn_openFile
            // 
            this.btn_openFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_openFile.Location = new System.Drawing.Point(317, 45);
            this.btn_openFile.Margin = new System.Windows.Forms.Padding(2);
            this.btn_openFile.Name = "btn_openFile";
            this.btn_openFile.Size = new System.Drawing.Size(71, 26);
            this.btn_openFile.TabIndex = 1;
            this.btn_openFile.Text = "Dosya &Aç";
            this.btn_openFile.UseVisualStyleBackColor = true;
            this.btn_openFile.Click += new System.EventHandler(this.btn_openFile_Click);
            // 
            // rtb_log
            // 
            this.rtb_log.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.rtb_log.AutoWordSelection = true;
            this.rtb_log.Location = new System.Drawing.Point(10, 180);
            this.rtb_log.Margin = new System.Windows.Forms.Padding(2);
            this.rtb_log.Name = "rtb_log";
            this.rtb_log.Size = new System.Drawing.Size(380, 266);
            this.rtb_log.TabIndex = 2;
            this.rtb_log.Text = "";
            // 
            // lbl_log
            // 
            this.lbl_log.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbl_log.AutoSize = true;
            this.lbl_log.Location = new System.Drawing.Point(11, 165);
            this.lbl_log.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_log.Name = "lbl_log";
            this.lbl_log.Size = new System.Drawing.Size(31, 13);
            this.lbl_log.TabIndex = 3;
            this.lbl_log.Text = "İşlem";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(9, 75);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(380, 2);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(11, 81);
            this.btn_apply.Margin = new System.Windows.Forms.Padding(2);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(380, 35);
            this.btn_apply.TabIndex = 5;
            this.btn_apply.Text = "UYGULA";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(9, 120);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(380, 2);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(9, 38);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(380, 2);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Visible = false;
            // 
            // cb_store
            // 
            this.cb_store.DisplayMember = "1";
            this.cb_store.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_store.Enabled = false;
            this.cb_store.FormattingEnabled = true;
            this.cb_store.Items.AddRange(new object[] {
            "Toffee Apple Book",
            "Powerseller Book"});
            this.cb_store.Location = new System.Drawing.Point(140, 12);
            this.cb_store.Name = "cb_store";
            this.cb_store.Size = new System.Drawing.Size(121, 21);
            this.cb_store.TabIndex = 8;
            this.cb_store.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "İşlem Yapılacak Mağaza";
            this.label1.Visible = false;
            // 
            // pb_status
            // 
            this.pb_status.Location = new System.Drawing.Point(11, 127);
            this.pb_status.Name = "pb_status";
            this.pb_status.Size = new System.Drawing.Size(380, 29);
            this.pb_status.TabIndex = 10;
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(11, 161);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(380, 2);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 450);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pb_status);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_store);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_log);
            this.Controls.Add(this.rtb_log);
            this.Controls.Add(this.btn_openFile);
            this.Controls.Add(this.tb_fileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Pi Store";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_fileName;
        private System.Windows.Forms.Button btn_openFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RichTextBox rtb_log;
        private System.Windows.Forms.Label lbl_log;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cb_store;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pb_status;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

