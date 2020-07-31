namespace ReadersHub.ProductAnalyzer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmi_file = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_fileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_settings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_store = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_criteria = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_about = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_File = new System.Windows.Forms.GroupBox();
            this.btn_openFile = new System.Windows.Forms.Button();
            this.tb_fileName = new System.Windows.Forms.TextBox();
            this.pb_status = new System.Windows.Forms.ProgressBar();
            this.btn_calculate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssl_remainingToken = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_remainingTokenValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_requestRestartDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_requestRestartDateValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.groupBox_File.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_file,
            this.tsmi_settings,
            this.tsmi_about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(522, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmi_file
            // 
            this.tsmi_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_fileOpen,
            this.toolStripSeparator1,
            this.tsmi_exit});
            this.tsmi_file.Name = "tsmi_file";
            this.tsmi_file.Size = new System.Drawing.Size(62, 24);
            this.tsmi_file.Text = "&Dosya";
            // 
            // tsmi_fileOpen
            // 
            this.tsmi_fileOpen.Name = "tsmi_fileOpen";
            this.tsmi_fileOpen.Size = new System.Drawing.Size(114, 26);
            this.tsmi_fileOpen.Text = "&Aç";
            this.tsmi_fileOpen.Click += new System.EventHandler(this.btn_openFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
            // 
            // tsmi_exit
            // 
            this.tsmi_exit.Name = "tsmi_exit";
            this.tsmi_exit.Size = new System.Drawing.Size(114, 26);
            this.tsmi_exit.Text = "&Çıkış";
            // 
            // tsmi_settings
            // 
            this.tsmi_settings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_store,
            this.tsmi_criteria});
            this.tsmi_settings.Name = "tsmi_settings";
            this.tsmi_settings.Size = new System.Drawing.Size(68, 24);
            this.tsmi_settings.Text = "&Ayarlar";
            // 
            // tsmi_store
            // 
            this.tsmi_store.Name = "tsmi_store";
            this.tsmi_store.Size = new System.Drawing.Size(192, 26);
            this.tsmi_store.Text = "Mağaza Ayarları";
            this.tsmi_store.Click += new System.EventHandler(this.tsmi_store_Click);
            // 
            // tsmi_criteria
            // 
            this.tsmi_criteria.Name = "tsmi_criteria";
            this.tsmi_criteria.Size = new System.Drawing.Size(192, 26);
            this.tsmi_criteria.Text = "Kriter Ayarları";
            this.tsmi_criteria.Click += new System.EventHandler(this.tsmi_criteria_Click);
            // 
            // tsmi_about
            // 
            this.tsmi_about.Name = "tsmi_about";
            this.tsmi_about.Size = new System.Drawing.Size(83, 24);
            this.tsmi_about.Text = "&Hakkında";
            this.tsmi_about.Click += new System.EventHandler(this.tsmi_about_Click);
            // 
            // groupBox_File
            // 
            this.groupBox_File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_File.Controls.Add(this.btn_openFile);
            this.groupBox_File.Controls.Add(this.tb_fileName);
            this.groupBox_File.ForeColor = System.Drawing.Color.White;
            this.groupBox_File.Location = new System.Drawing.Point(12, 32);
            this.groupBox_File.Name = "groupBox_File";
            this.groupBox_File.Size = new System.Drawing.Size(498, 71);
            this.groupBox_File.TabIndex = 1;
            this.groupBox_File.TabStop = false;
            this.groupBox_File.Text = "Dosya";
            // 
            // btn_openFile
            // 
            this.btn_openFile.ForeColor = System.Drawing.Color.Black;
            this.btn_openFile.Location = new System.Drawing.Point(400, 21);
            this.btn_openFile.Name = "btn_openFile";
            this.btn_openFile.Size = new System.Drawing.Size(92, 30);
            this.btn_openFile.TabIndex = 1;
            this.btn_openFile.Text = "Dosya Seç";
            this.btn_openFile.UseVisualStyleBackColor = true;
            this.btn_openFile.Click += new System.EventHandler(this.btn_openFile_Click);
            // 
            // tb_fileName
            // 
            this.tb_fileName.BackColor = System.Drawing.Color.White;
            this.tb_fileName.Location = new System.Drawing.Point(6, 21);
            this.tb_fileName.Multiline = true;
            this.tb_fileName.Name = "tb_fileName";
            this.tb_fileName.ReadOnly = true;
            this.tb_fileName.Size = new System.Drawing.Size(388, 30);
            this.tb_fileName.TabIndex = 0;
            // 
            // pb_status
            // 
            this.pb_status.Location = new System.Drawing.Point(12, 109);
            this.pb_status.Name = "pb_status";
            this.pb_status.Size = new System.Drawing.Size(498, 30);
            this.pb_status.Step = 1;
            this.pb_status.TabIndex = 2;
            // 
            // btn_calculate
            // 
            this.btn_calculate.Location = new System.Drawing.Point(12, 160);
            this.btn_calculate.Name = "btn_calculate";
            this.btn_calculate.Size = new System.Drawing.Size(498, 35);
            this.btn_calculate.TabIndex = 3;
            this.btn_calculate.Text = "&HESAPLA";
            this.btn_calculate.UseVisualStyleBackColor = true;
            this.btn_calculate.Click += new System.EventHandler(this.btn_calculate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 147);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 4);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_remainingToken,
            this.tssl_remainingTokenValue,
            this.tssl_requestRestartDate,
            this.tssl_requestRestartDateValue});
            this.statusStrip1.Location = new System.Drawing.Point(0, 206);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(522, 25);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssl_remainingToken
            // 
            this.tssl_remainingToken.ForeColor = System.Drawing.Color.White;
            this.tssl_remainingToken.Name = "tssl_remainingToken";
            this.tssl_remainingToken.Size = new System.Drawing.Size(98, 20);
            this.tssl_remainingToken.Text = "Kalan Kontör:";
            // 
            // tssl_remainingTokenValue
            // 
            this.tssl_remainingTokenValue.ForeColor = System.Drawing.Color.White;
            this.tssl_remainingTokenValue.Name = "tssl_remainingTokenValue";
            this.tssl_remainingTokenValue.Size = new System.Drawing.Size(17, 20);
            this.tssl_remainingTokenValue.Text = "0";
            // 
            // tssl_requestRestartDate
            // 
            this.tssl_requestRestartDate.ForeColor = System.Drawing.Color.White;
            this.tssl_requestRestartDate.Name = "tssl_requestRestartDate";
            this.tssl_requestRestartDate.Size = new System.Drawing.Size(134, 20);
            this.tssl_requestRestartDate.Text = "| Yenileme Zamanı:";
            // 
            // tssl_requestRestartDateValue
            // 
            this.tssl_requestRestartDateValue.ForeColor = System.Drawing.Color.White;
            this.tssl_requestRestartDateValue.Name = "tssl_requestRestartDateValue";
            this.tssl_requestRestartDateValue.Size = new System.Drawing.Size(118, 20);
            this.tssl_requestRestartDateValue.Text = "10.02.2017 10:35";
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(66)))), ((int)(((byte)(121)))));
            this.ClientSize = new System.Drawing.Size(522, 231);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_calculate);
            this.Controls.Add(this.pb_status);
            this.Controls.Add(this.groupBox_File);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Product Analyzer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox_File.ResumeLayout(false);
            this.groupBox_File.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_file;
        private System.Windows.Forms.ToolStripMenuItem tsmi_fileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_exit;
        private System.Windows.Forms.ToolStripMenuItem tsmi_settings;
        private System.Windows.Forms.ToolStripMenuItem tsmi_about;
        private System.Windows.Forms.GroupBox groupBox_File;
        private System.Windows.Forms.Button btn_openFile;
        private System.Windows.Forms.TextBox tb_fileName;
        private System.Windows.Forms.ProgressBar pb_status;
        private System.Windows.Forms.Button btn_calculate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_store;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem tsmi_criteria;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_remainingToken;
        private System.Windows.Forms.ToolStripStatusLabel tssl_remainingTokenValue;
        private System.Windows.Forms.ToolStripStatusLabel tssl_requestRestartDate;
        private System.Windows.Forms.ToolStripStatusLabel tssl_requestRestartDateValue;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

