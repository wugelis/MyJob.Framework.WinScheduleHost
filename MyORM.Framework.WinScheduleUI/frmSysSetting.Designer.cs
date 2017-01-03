namespace MyORM.Framework.WinScheduleUI
{
    partial class frmSysSetting
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
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvJobStatus = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labAvariableJobs = new System.Windows.Forms.Label();
            this.labCurrentRunning = new System.Windows.Forms.Label();
            this.timerUpdateData = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvJobStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 264);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "排程的狀態預設每秒鐘更新一次。 ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvJobStatus);
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 194);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "目前系統中有效排程的執行狀態";
            // 
            // gvJobStatus
            // 
            this.gvJobStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvJobStatus.Location = new System.Drawing.Point(7, 22);
            this.gvJobStatus.Name = "gvJobStatus";
            this.gvJobStatus.RowTemplate.Height = 24;
            this.gvJobStatus.Size = new System.Drawing.Size(408, 150);
            this.gvJobStatus.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(367, 288);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 54);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "關閉(C&)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(259, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "有效排程：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(259, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "執行中的排程：";
            // 
            // labAvariableJobs
            // 
            this.labAvariableJobs.AutoSize = true;
            this.labAvariableJobs.Location = new System.Drawing.Point(402, 19);
            this.labAvariableJobs.Name = "labAvariableJobs";
            this.labAvariableJobs.Size = new System.Drawing.Size(11, 12);
            this.labAvariableJobs.TabIndex = 7;
            this.labAvariableJobs.Text = "0";
            this.labAvariableJobs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labCurrentRunning
            // 
            this.labCurrentRunning.AutoSize = true;
            this.labCurrentRunning.Location = new System.Drawing.Point(402, 42);
            this.labCurrentRunning.Name = "labCurrentRunning";
            this.labCurrentRunning.Size = new System.Drawing.Size(11, 12);
            this.labCurrentRunning.TabIndex = 8;
            this.labCurrentRunning.Text = "0";
            this.labCurrentRunning.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerUpdateData
            // 
            this.timerUpdateData.Interval = 1000;
            this.timerUpdateData.Tick += new System.EventHandler(this.timerUpdateData_Tick);
            // 
            // frmSysSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 353);
            this.Controls.Add(this.labCurrentRunning);
            this.Controls.Add(this.labAvariableJobs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Name = "frmSysSetting";
            this.Text = "查看排程執行狀態";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvJobStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView gvJobStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labAvariableJobs;
        private System.Windows.Forms.Label labCurrentRunning;
        private System.Windows.Forms.Timer timerUpdateData;
    }
}